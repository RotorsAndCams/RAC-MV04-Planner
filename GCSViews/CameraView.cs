using CoordinateSharp;
using GMap.NET;
using log4net;
using log4net.Repository.Hierarchy;
using MissionPlanner.Utilities;
using MV04.Camera;
using MV04.Settings;
using MV04.State;
using MV04.TestForms;
using NetTopologySuite.Operation.Valid;
using NextVisionVideoControlLibrary;
using OpenTK.Graphics.ES11;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.RightsManagement;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;
using static MV04.Camera.MavProto;
using Accord.Video.FFMPEG;
using MV04.SingleYaw;

namespace MissionPlanner.GCSViews
{
    public partial class CameraView : MyUserControl//, IActivate, IDeactivate
    {
        #region Fields

        public static CameraView instance;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        string CameraIP;
        int CameraStreamChannel;
        new Font DefaultFont;
        Brush DefaultBrush;
        Rectangle VideoRectangle;
        Graphics gr;
        HudElements HudElements = new HudElements();

        System.Timers.Timer FetchHudDataTimer = new System.Timers.Timer();

        (int major, int minor, int build) CameraControlDLLVersion;

        Random rnd = new Random();
        bool OSDDebug = true;
        string[] OSDDebugLines = new string[10];

        private CameraFullScreenForm _cameraFullScreenForm;
        private bool _isFPVModeActive = false;
        public static Size Trip5Size = new Size(1920, 1200);
        public static bool IsCameraTrackingModeActive { get; set; } = false;

        System.Timers.Timer _droneStatusTimer;
        System.Timers.Timer _cameraSwitchOffTimer;

        public const int _maxAllowedAltitudeValue = 500;
        public const int _minAllowedAltitudeValue = 50;

        private bool _tripSwitchedOff = false;

        Image img;
        private readonly object _bgimagelock = new object();

        Bitmap _actualCameraImage;

        int _frameRate = 25;
        bool _recordingInProgress = false;
        int _segmentLength;

        string _tempPath = "";
        int _fileCount = 0;
        NvSystemModes _cameraState;

        #endregion

        #region Conversion multipliers
        const double Meter_to_Feet = 3.2808399;
        const double Mps_to_Kmph = 3.6;
        const double Mps_to_Knots = 1.94384449;
        #endregion

        #region Constructor

        public CameraView()
        {
            MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_RELAY, CameraHandler.TripChannelNumber, 1, 0, 0, 0, 0, 0);

            log.Info("Constructor");
            InitializeComponent();
            instance = this;

            // Camera
            CameraIP = SettingManager.Get(Setting.CameraIP);
            CameraStreamChannel = int.Parse(SettingManager.Get(Setting.CameraStreamChannel));
            FetchHudDataTimer.Interval = 100; // 10Hz
            FetchHudDataTimer.Elapsed += (sender, eventArgs) => FetchHudData();
            CameraControlDLLVersion = CameraHandler.Instance.CameraControlDLLVersion;

            // Create default drawing objects
            DefaultFont = new Font(FontFamily.GenericMonospace, this.Font.SizeInPoints * 2f);
            DefaultBrush = new SolidBrush(Color.Red);

            // Snapshot & video save location
            CameraHandler.Instance.MediaSavePath = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_media" + Path.DirectorySeparatorChar;

            // SysID for camera functions
            CameraHandler.sysID = MainV2.comPort.sysidcurrent;
            MainV2.comPort.MavChanged += (sender, eventArgs) => CameraHandler.sysID = MainV2.comPort.sysidcurrent; // Update sysID on new connection

            // Connect to camera
            StartCameraStream();
            StartCameraControl();
            CameraHandler.Instance.event_ReportArrived += CameraHandler_event_ReportArrived;
            CameraHandler.Instance.event_DoPhoto += Instance_event_DoPhoto;

            // Start single yaw
            SingleYawHandler.StartSingleYaw(MainV2.comPort);

            // Draw UI
            DrawUI();
            DisableControls();
            SetStopButtonVisibility();

            //States
            SetDroneStatusValue();
            this.Resize += CameraView_Resize;
            SetDroneLEDStates(enum_LandingLEDState.Off, enum_PositionLEDState_IR.Off, enum_PositionLEDState_RedLight.Off);
            LEDStateHandler.LedStateChanged += LEDStateHandler_LedStateChanged;

            StateHandler.MV04StateChange += StateHandler_MV04StateChange;

            if (!CameraHandler.Instance.HasCameraReport(MavReportType.SystemReport))
                this.lb_CameraStatusValue.Text = "NoCom";

            _droneStatusTimer = new System.Timers.Timer();
            _droneStatusTimer.Elapsed += _droneStatustimer_Elapsed;
            _droneStatusTimer.Interval = 3000;
            _droneStatusTimer.Enabled = true;

            if((int)MainV2.comPort.MAV.cs.alt < _minAllowedAltitudeValue)
                this.cs_ColorSliderAltitude.Value = _minAllowedAltitudeValue;
            else if((int)MainV2.comPort.MAV.cs.alt > _maxAllowedAltitudeValue)
                this.cs_ColorSliderAltitude.Value -= _maxAllowedAltitudeValue;
            else
                this.cs_ColorSliderAltitude.Value = (int)MainV2.comPort.MAV.cs.alt;

            //timer for camera switchoff
            _cameraSwitchOffTimer = new System.Timers.Timer();
            _cameraSwitchOffTimer.Elapsed += _cameraSwitchOffTimer_Elapsed;
            _cameraSwitchOffTimer.Interval = 30000;
            _cameraSwitchOffTimer.Enabled = true;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            this.DoubleBuffered = true;
            pb_CameraGstream.Paint += Pb_CameraGstream_Paint;

            GStreamer.onNewImage += (sender, image) =>
            {
                try
                {
                    if (image == null) return;

                    img = new Bitmap(image.Width, image.Height, 4 * image.Width,
                                System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888)
                                    .Scan0);

                    if (img == null) return;

                    if (InvokeRequired)
                        Invoke(new Action(() => pb_CameraGstream.Invalidate()));
                    else
                        pb_CameraGstream.Invalidate();

                    //
                    //Task.Factory.StartNew(() => {

                        

                    //});
                    

                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Gst error" + ex.Message);
                }
            };

            this.FormClosing += CameraView_FormClosing;
            
            _videoRecordTimer.Interval = 40;
            _videoRecordTimer.Elapsed += _videoRecordTimer_Tick;
            _segmentLength = int.Parse(SettingManager.Get(Setting.VideoSegmentLength));

            pb_CameraGstream.Invalidate();

            bool autoRecord = bool.Parse(SettingManager.Get(Setting.AutoRecordVideoStream));

            if (autoRecord)
            {
                StartRecording();
            }

        }

        

        #endregion

        #region Init

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.A))
            {
                tlp_AGLContainer.Visible = !tlp_AGLContainer.Visible;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DisableControls()
        {
            this.tlp_AGLContainer.Visible = false;
        }

        /// <summary>
        /// Draws UI elements
        /// </summary>
        private void DrawUI()
        {
            CameraSettingsForm.Instance.event_ReconnectRequested += Form_event_ReconnectRequested;
            CameraSettingsForm.Instance.event_StartStopRecording += CameraSettings_event_StartStopRecording;
            
            // Test functions
            #region Test functions

            Dictionary<string, Action> testFunctions = new Dictionary<string, Action>
            {
                {"Open settings", () => { SettingManager.OpenDialog(); }},
                {"Start stream", () => { StartCameraStream(); }},
                {"Start control", () => { StartCameraControl(); }},
                {"Start stream & control", () => { StartCameraStream(); StartCameraControl(); }},
                {"Switch crosshairs", () => { ChangeCrossHair(); }},
                {"Do photo", () => { DoPhoto(); }},
                {"Start recording (loop)", () => { StartRecording(); }},
                {"Stop recording", () => { StopRecording(); }},
                {"Set mode", () => { new CameraModeSelectorForm().Show(); }},
                {"Tracker mode", () => { new TrackerPosForm().Show(); }},
                {"Camera mover", () => { new CameraMoverForm().Show(); }},
                {"Reset zoom", async () => { await ResetZoom(); }},
                {"Toggle Day/Night", async () => { await ToggleDayNightCamera(); }},
                {"Toggle IR Color", async () => { await ToggleIRColor(); }},
                {"Toggle IR Polarity", async () => { await ToggleIRPolarity(); }},
                {"Do BIT", async () => { await DoBIT(); }},
                {"Do NUC", async () => { await DoNUC(); }},
                {"Test Gstreamer", async () => { new GstreamerTestForm().Show(); }},
                {"Test GCS Mode", async () => { new GCSModeTesterForm().Show(); }},
                {"Joystick axis switcher", async () => { new  JoystickAxisSwitcherForm(MainV2.joystick).ShowDialog(); }},
                {"Start single-yaw loop", async () => { SingleYawHandler.StartSingleYaw(MainV2.comPort); }},
                {"Stop single-yaw loop", async () => { SingleYawHandler.StopSingleYaw(); }},
                {"Open single-yaw", async () => { new SingleYawForm(MainV2.comPort).Show(); }},
            };

            #endregion

            #region Dev debug tools

#if DEBUG

            ComboBox cb_TestFunctions = new ComboBox();
            cb_TestFunctions.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_TestFunctions.Items.AddRange(testFunctions.Keys.ToArray());
            cb_TestFunctions.SelectedIndex = 0;
            cb_TestFunctions.Location = new Point(10, (this.Height / 3) + 0);
            cb_TestFunctions.Width = 100;
            this.Controls.Add(cb_TestFunctions);
            cb_TestFunctions.BringToFront();

            Button bt_DoTestFunction = new Button();
            bt_DoTestFunction.Text = "Test function";
            bt_DoTestFunction.Location = new Point(10, (this.Height / 3) + 25);
            bt_DoTestFunction.Width = 100;
            bt_DoTestFunction.Click += (sender, e) => testFunctions[cb_TestFunctions.SelectedItem.ToString()]();
            this.Controls.Add(bt_DoTestFunction);
            bt_DoTestFunction.BringToFront();
#endif

            #endregion
        }

        #endregion

        #region CameraFunctions

        private void StartCameraStream()
        {
            bool success = StartGstreamerCameraStream(CameraHandler.url);

            if (success)
            {
                FetchHudData();
                FetchHudDataTimer.Start();

#if DEBUG
                AddToOSDDebug("Video stream started");
#endif
            }
            else
            {
#if DEBUG
                AddToOSDDebug("Failed start video stream");
#endif
            }
        }

        private void StartCameraControl()
        {
            bool success = CameraHandler.Instance.CameraControlConnect(
                IPAddress.Parse(SettingManager.Get(Setting.CameraIP)),
                int.Parse(SettingManager.Get(Setting.CameraControlPort)));

#if DEBUG
            if (success)
                AddToOSDDebug("Camera control started");
            else
                AddToOSDDebug("Failed to start camera control");
#endif
        }

        private void ReconnectCameraStreamAndControl()
        {
            StartCameraStream();
            StartCameraControl();
        }

        #region Crosshair

        private void ChangeCrossHair()
        {
            SetCrosshairType(HudElements.Crosshairs == CrosshairsType.Plus ? CrosshairsType.HorizontalDivisions : CrosshairsType.Plus);

#if DEBUG
            AddToOSDDebug("Crosshairs set to " + Enum.GetName(typeof(CrosshairsType), HudElements.Crosshairs));
#endif
        }

        /// <summary>
        /// Set the crosshair type on the OSD
        /// </summary>
        /// <param name="type">Crosshair type to set</param>
        private void SetCrosshairType(CrosshairsType type)
        {
            if (HudElements.Crosshairs != type)
            {
                HudElements.Crosshairs = type;
            }
        }

        #endregion

        private void DoPhoto(string path = null)
        {
            _actualCameraImage = new Bitmap(pb_CameraGstream.Width, pb_CameraGstream.Height);
            pb_CameraGstream.DrawToBitmap(_actualCameraImage, new Rectangle(0, 0, pb_CameraGstream.Width, pb_CameraGstream.Height));

            if (path == null)
                path = CameraHandler.Instance.MediaSavePath + "test" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";

            if (_actualCameraImage != null)
                _actualCameraImage.Save(path, ImageFormat.Jpeg);

#if DEBUG
            AddToOSDDebug("Photo taken");
#endif
        }

        object _lockImageSaveTimer = new object();
        System.Timers.Timer _videoRecordTimer = new System.Timers.Timer();
        private VideoRecorder _videoRecorder = new VideoRecorder();

        /// <summary>
        /// Add bitmap to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _videoRecordTimer_Tick(object sender, EventArgs e)
        {
            lock (_lockImageSaveTimer)
            {
                if (_recordingInProgress)
                {

                    try
                    {
                        var _actualCameraVideoImage = new Bitmap(1920, 1080);

                        Invoke((MethodInvoker)delegate { pb_CameraGstream.DrawToBitmap(_actualCameraVideoImage, new Rectangle(0, 0, 1920, 1080)); });

                        _videoRecorder.AddNewImage(_actualCameraVideoImage);
                    }
                    catch { }



                }
                else
                {
                    _videoRecordTimer.Stop();
                    _videoRecordTimer.Close();
                }

            }
        }

        private void StartRecording()
        {
            int sl = int.Parse(SettingManager.Get(Setting.VideoSegmentLength));

            _videoRecordTimer?.Start();
            _videoRecorder.Start();

            _recordingInProgress = true;
        }

        private void StopRecording()
        {
            _recordingInProgress = false;
            _videoRecordTimer?.Stop();
        }

        private async Task ResetZoom()
        {
            bool success = CameraHandler.Instance.ResetZoom();

#if DEBUG

            if (success)
                AddToOSDDebug("Zoom reset");
            else
                AddToOSDDebug("Zoom reset failed");
#endif
        }

        private async Task ToggleDayNightCamera()
        {
            bool success;

            if (!CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport) || ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).activeSensor == 1) // Unknown / Night Vision
            {
                success = CameraHandler.Instance.SetImageSensor(false); // Set to Day

#if DEBUG
                if (success)
                    AddToOSDDebug("Camera sensor set to day");
                else
                    AddToOSDDebug("Camera sensor set failed");
#endif
            }
            else // Day
            {
                success = CameraHandler.Instance.SetImageSensor(true); // Set to Night Vision

#if DEBUG
                if (success)
                    AddToOSDDebug("Camera sensor set to night");
                else
                    AddToOSDDebug("Camera sensor set failed");
#endif
            }
        }

        private async Task ToggleIRColor()
        {
            bool success;

            if (CameraHandler.Instance.CurrentIRColor == IRColor.Grayscale) // Grayscale
            {
                success = CameraHandler.Instance.SetIRColor(IRColor.Color); // Set to color

#if DEBUG
                if (success)
                    AddToOSDDebug("Camera IR color set to Color");
                else
                    AddToOSDDebug("Camera IR color set failed");
#endif
            }
            else // Color
            {
                success = CameraHandler.Instance.SetIRColor(IRColor.Grayscale); // Set to grayscale

#if DEBUG
                if (success)
                    AddToOSDDebug("Camera IR color set to Grayscale");
                else
                    AddToOSDDebug("Camera IR color set failed");
#endif
            }
        }

        private async Task ToggleIRPolarity()
        {
            bool success;

            if (!CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport) || ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).irSensorPolarity == 1) // Unknown / Inverted
            {
                success = CameraHandler.Instance.SetIRPolarity(IRPolarity.Normal);
#if DEBUG
                if (success)
                    AddToOSDDebug("Camera IR polarity set to Normal");
                else
                    AddToOSDDebug("Camera IR polarity set failed");
#endif
            }
            else // Normal
            {
                success = CameraHandler.Instance.SetIRPolarity(IRPolarity.Inverted);
#if DEBUG
                if (success)
                    AddToOSDDebug("Camera IR polarity set to Inverted");
                else
                    AddToOSDDebug("Camera IR polarity set failed");
#endif
            }
        }

        private async Task DoBIT()
        {
            bool success = CameraHandler.Instance.DoBIT();

#if DEBUG
            if (success)
                AddToOSDDebug("Doing built-in test");
            else
                AddToOSDDebug("Built-in test failed");
#endif
        }

        private async Task DoNUC()
        {
            bool success = CameraHandler.Instance.DoNUC();

#if DEBUG
            if (success)
                AddToOSDDebug("NV calibrated");
            else
                AddToOSDDebug("NV calibration failed");
#endif
        }

        private bool StartGstreamerCameraStream(string p_url)
        {
            try
            {
                CameraHandler.Instance.StartGstreamer(p_url);
                //GStreamer.StartA(p_url);
                return true;
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString(), Strings.ERROR);
                return false;
            }
        }

        #endregion

        #region Drawing

        private void OnNewFrame(int width, int height, Graphics _gr)
        {
            // frame_buf is 1920 x 1080 x 3 long
            // real frame is width x height

            // Create drawing objects
            
            _gr.InterpolationMode = InterpolationMode.High;
            _gr.SmoothingMode = SmoothingMode.HighQuality;
            _gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            _gr.CompositingQuality = CompositingQuality.HighQuality;
            VideoRectangle = new Rectangle()
            {
                X = (int)Math.Round(_gr.VisibleClipBounds.X),
                Y = (int)Math.Round(_gr.VisibleClipBounds.Y),
                Width = (int)Math.Round(_gr.VisibleClipBounds.Width),
                Height = (int)Math.Round(_gr.VisibleClipBounds.Height)
            };

            // Datetime
            Rectangle Datetime = DrawText(HudElements.Time, new Point(3, 3), ContentAlignment.TopLeft, HorizontalAlignment.Left, null,null,null, _gr);

            // Battery
            Rectangle Battery = DrawText(HudElements.Battery, new Point(VideoRectangle.Width - 3, 3), ContentAlignment.TopRight, HorizontalAlignment.Right, null, null, null, _gr);

            int topLeft = Datetime.Right;
            int topStep = ((Battery.Left - topLeft) / 4) / 2;

            // AGL
            DrawText(HudElements.AGL, new Point(topLeft + topStep, 3), ContentAlignment.TopCenter, HorizontalAlignment.Left, null, null, null, _gr);

            // Velocity
            DrawText(HudElements.Velocity, new Point(topLeft + (3 * topStep), 3), ContentAlignment.TopCenter, HorizontalAlignment.Left, null, null, null, _gr);

            // TGD
            DrawText(HudElements.TGD, new Point(topLeft + (5 * topStep), 3), ContentAlignment.TopCenter, HorizontalAlignment.Left, null, null, null, _gr);

            // Signal strengths
            DrawText(HudElements.SignalStrengths, new Point(topLeft + (7 * topStep), 3), ContentAlignment.TopCenter, HorizontalAlignment.Right, null, null, null, _gr);

            // Camera info
            DrawText(HudElements.Camera, new Point(3, Datetime.Bottom + 20), ContentAlignment.TopLeft, HorizontalAlignment.Right, null, null, null, _gr);

            // Next waypoint
            Rectangle nextWP = DrawText(HudElements.ToWaypoint, new Point(VideoRectangle.Width - 3, Battery.Bottom + 20), ContentAlignment.TopRight, HorizontalAlignment.Right, null, null, null, _gr);

            // Operator distance
            DrawText(HudElements.FromOperator, new Point(VideoRectangle.Width - 3, nextWP.Bottom + 20), ContentAlignment.TopRight, HorizontalAlignment.Right, null, null, null, _gr);

            // Coords
            DrawText(HudElements.DroneGps, new Point(0, VideoRectangle.Height - 3), ContentAlignment.BottomLeft, HorizontalAlignment.Left, null, null, null, _gr);
            DrawText(HudElements.TargetGps, new Point(VideoRectangle.Width - 3, VideoRectangle.Height - 3), ContentAlignment.BottomRight, HorizontalAlignment.Right, null, null, null, _gr);

            #region Crosshairs
            int lineHeight = (int)Math.Round(VideoRectangle.Height * 0.1);
            Pen linePen = new Pen(Color.Red, 1);

            if (HudElements.Crosshairs == CrosshairsType.Plus) // Plus
            {
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) + lineHeight, VideoRectangle.Height / 2);
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    VideoRectangle.Width / 2, (VideoRectangle.Height / 2) + lineHeight);
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) - lineHeight, VideoRectangle.Height / 2);
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    VideoRectangle.Width / 2, (VideoRectangle.Height / 2) - lineHeight);
            }
            else // Horizontal
            {
                // Draw center ^ character
                _gr.DrawLine(new Pen(Color.Red, 3),
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) - Math.Min(lineHeight / 2, HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);
                _gr.DrawLine(new Pen(Color.Red, 3),
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) + Math.Min(lineHeight / 2, HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);

                for (int i = 1; i <= 3; i++)
                {
                    // Draw lines to the right
                    _gr.DrawLine(linePen,
                        (VideoRectangle.Width / 2) + (i * HudElements.LineSpacing), VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) + (i * HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);

                    // Draw lines to the left
                    _gr.DrawLine(linePen,
                        (VideoRectangle.Width / 2) - (i * HudElements.LineSpacing), VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) - (i * HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);
                }

                // Draw number under first right line
                DrawText(HudElements.LineDistance.ToString(), new Point((VideoRectangle.Width / 2) + HudElements.LineSpacing, (VideoRectangle.Height / 2) + lineHeight + 3), ContentAlignment.TopCenter, HorizontalAlignment.Center, new Font(DefaultFont.FontFamily, this.Font.SizeInPoints, FontStyle.Regular));
            }
            #endregion

            // OSDDebug
            if (OSDDebug && !string.IsNullOrWhiteSpace(OSDDebugLines[0]))
            {
                string text = OSDDebugLines[0];
                for (int i = 1; i < OSDDebugLines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(OSDDebugLines[i]))
                    {
                        text += "\n" + OSDDebugLines[i];
                    }
                }

                DrawText(text, new Point(VideoRectangle.Width - 3, VideoRectangle.Height / 2), ContentAlignment.TopRight, HorizontalAlignment.Left, new Font(DefaultFont.FontFamily, DefaultFont.Size * 0.75f), Brushes.Lime);
            }
        }

        /// <summary>
        /// Pixel count for a given horizontal distance (in meters) at the camera target
        /// </summary>
        /// <param name="slantRange">Camera target distance in meters</param>
        /// <param name="fovDegrees">Camera field of view in degrees</param>
        /// <param name="fovPixels">Camera field of view in pixels (video horizontal resolution)</param>
        /// <param name="hMeters">Desired horizontal distance for the return value</param>
        /// <returns>Pixel count for a given horizontal distance (in meters) at the camera target</returns>
        private int PixelsForMeters(double slantRange, double fovDegrees, int fovPixels, int hMeters = 10)
        {
            double fovMeters = 2.0 * slantRange * Math.Tan(MathHelper.Radians(fovDegrees / 2.0));
            int pixelPerMeter = (int)Math.Round((double)fovPixels / fovMeters); // Use Math.Ceiling() instead?
            return pixelPerMeter * hMeters;
        }

        /// <summary>
        /// Draws a text on the control at a given location
        /// </summary>
        private Rectangle DrawText(string text, Point position, ContentAlignment rectangleAlignment = ContentAlignment.TopLeft, HorizontalAlignment textAlignment = HorizontalAlignment.Left, Font textFont = null, Brush textBrush = null, Rectangle? drawArea = null, Graphics drawGraphics = null)
        {
            // Null check text
            text = text ?? "";

            // Set nullables
            textFont = textFont ?? DefaultFont;
            textBrush = textBrush ?? DefaultBrush;
            drawArea = drawArea ?? VideoRectangle;
            drawGraphics = drawGraphics != null ? drawGraphics : this.CreateGraphics();

            // Check position
            if (position.X >= 0
                && position.X <= drawArea.Value.Width
                && position.Y >= 0
                && position.Y <= drawArea.Value.Height)
            {
                // Draw text
                StringAlignment textHorizontalAlignment = StringAlignment.Near; // Relative to top left corner
                switch (textAlignment)
                {
                    case HorizontalAlignment.Center:
                        textHorizontalAlignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        textHorizontalAlignment = StringAlignment.Far;
                        break;
                    default: // HorizontalAlignment.Left
                        break;
                }
                StringFormat textFormat = new StringFormat()
                {
                    Alignment = textHorizontalAlignment,
                    LineAlignment = StringAlignment.Center, // Relative to top left corner
                    FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip
                };

                Size textSize = TextSize(text, textFont, drawGraphics);
                switch (rectangleAlignment)
                {
                    case ContentAlignment.TopCenter:
                        position.X -= textSize.Width / 2;
                        break;
                    case ContentAlignment.TopRight:
                        position.X -= textSize.Width + 1;
                        break;
                    case ContentAlignment.MiddleLeft:
                        position.Y -= textSize.Height / 2;
                        break;
                    case ContentAlignment.MiddleCenter:
                        position.X -= textSize.Width / 2;
                        position.Y -= textSize.Height / 2;
                        break;
                    case ContentAlignment.MiddleRight:
                        position.X -= textSize.Width + 1;
                        position.Y -= textSize.Height / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                        position.Y -= textSize.Height + 1;
                        break;
                    case ContentAlignment.BottomCenter:
                        position.X -= textSize.Width / 2;
                        position.Y -= textSize.Height + 1;
                        break;
                    case ContentAlignment.BottomRight:
                        position.X -= textSize.Width + 1;
                        position.Y -= textSize.Height + 1;
                        break;
                    default: // ContentAlignment.TopLeft
                        break;
                }
                Rectangle textRectangle = new Rectangle()
                {
                    Size = textSize,
                    Location = position
                };

                // Draw text on control
                drawGraphics.DrawString(text, textFont, textBrush, textRectangle, textFormat);

                // Return rectangle
                return textRectangle;
            }
            else
            {
                return new Rectangle();
            }
        }

        /// <summary>
        /// Calculates the bounding rectangle size for a text
        /// </summary>
        private Size TextSize(string text, Font font, Graphics graphics)
        {
            SizeF size = graphics.MeasureString(text.Split('\n').OrderByDescending(s => s.Length).FirstOrDefault(), font);
            return new Size()
            {
                Width = (int)Math.Ceiling(size.Width) + 4,
                Height = (int)Math.Ceiling(size.Height * (text.Count(c => c == '\n') + 1))
            };
        }

        /// <summary>
        /// Add a new line to the OSD debug text
        /// </summary>
        /// <param name="line">Text line to add</param>
        private void AddToOSDDebug(string line)
        {
#if DEBUG
            // Shift everything down
            for (int i = OSDDebugLines.Length - 1; i > 0; i--)
            {
                OSDDebugLines[i] = OSDDebugLines[i - 1];
            }

            // Add new line
            OSDDebugLines[0] = line;
#endif
        }

        #endregion

        #region HUD

        /// <summary>
        /// Fetches fresh data for the overlay elements
        /// </summary>
        private void FetchHudData()
        {
            CurrentState cs = MainV2.comPort.MAV.cs;

            // Date and time
            DateTime now = DateTime.Now;
            int utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(now).Hours;
            HudElements.Time = $"{now.Day.ToString().PadLeft(2, '0')}{now.ToString("MMM", new CultureInfo("en-US")).ToUpperInvariant()}{now.Year}\n{now.ToString("HH:mm:ss")}\nUTC{(utcOffset >= 0 ? "+" : "")}{utcOffset}";

            /// Above Ground Level
            HudElements.AGL = "AGL";
            switch (SettingManager.Get(Setting.AltFormat))
            {
                case "ft":
                    HudElements.AGL += ((int)Math.Round(cs.alt * Meter_to_Feet)).ToString().PadLeft(4);
                    break;
                default: // "m"
                    HudElements.AGL += ((int)Math.Round(cs.alt)).ToString().PadLeft(4); // cs.alt is in meters
                    break;
            }
            HudElements.AGL += SettingManager.Get(Setting.AltFormat).ToUpper();

            // Horizontal velocity (ground speed)
            HudElements.Velocity = "VEL";
            switch (SettingManager.Get(Setting.SpeedFormat))
            {
                case "kmph":
                    HudElements.Velocity += ((int)Math.Round(cs.groundspeed * Mps_to_Kmph)).ToString().PadLeft(4) + "KM/H";
                    break;
                case "knots":
                    HudElements.Velocity += ((int)Math.Round(cs.groundspeed * Mps_to_Knots)).ToString().PadLeft(4) + "KTS";
                    break;
                default: // mps
                    HudElements.Velocity += ((int)Math.Round(cs.groundspeed)).ToString().PadLeft(4) + "M/S"; // cs.groundspeed is in m/s
                    break;
            }

            // Target distance (slant range)
            HudElements.TGD = "TGD";
            bool hasGndCrsRep = CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.GndCrsReport);
            double slantRange = hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsSlantRange : 0;
            switch (SettingManager.Get(Setting.DistFormat))
            {
                case "km":
                    HudElements.TGD += ((int)Math.Round(slantRange * 1000)).ToString().PadLeft(5);
                    break;
                case "ft":
                    HudElements.TGD += ((int)Math.Round(slantRange * Meter_to_Feet)).ToString().PadLeft(5);
                    break;
                default: // m
                    HudElements.TGD += ((int)Math.Round(slantRange)).ToString().PadLeft(5);
                    break;
            }
            HudElements.TGD += SettingManager.Get(Setting.DistFormat).ToUpper();

            // Battery percentage
            HudElements.Battery = "BAT"
                + cs.battery_remaining.ToString().PadLeft(4) + "%"; // Percentage
            //HudElements.Battery += $"\n00:{rnd.Next(0, 60).ToString().PadLeft(2, '0')}:{rnd.Next(0, 60).ToString().PadLeft(2, '0')}"; // Remaining time

            // Radio & GPS signal strength
            HudElements.SignalStrengths = "RADIO" + cs.linkqualitygcs.ToString().PadLeft(8) + "%";  // Radio signal percentage
            string gpsStr;
            switch (cs.gpsstatus)
            {
                case 0: gpsStr = "NO GPS"; break;
                case 1: gpsStr = "NO FIX"; break;
                case 2: gpsStr = "2D FIX"; break;
                case 3: gpsStr = "3D FIX"; break;
                case 4: gpsStr = "DGPS FIX"; break;
                case 5: gpsStr = "RTK LOW"; break;
                case 6: gpsStr = "RTK FIX"; break;
                default: gpsStr = cs.gpsstatus.ToString(); break;
            }
            HudElements.SignalStrengths += "\nGPS" + gpsStr.PadLeft(11);                            // GPS signal percentage

            // Operator (home) distance
            HudElements.FromOperator = "OPERATOR";
            switch (SettingManager.Get(Setting.DistFormat))
            {
                case "km":
                    HudElements.FromOperator += ((int)Math.Round(cs.DistToHome * 1000)).ToString().PadLeft(5);
                    break;
                case "ft":
                    HudElements.FromOperator += ((int)Math.Round(cs.DistToHome * Meter_to_Feet)).ToString().PadLeft(5);
                    break;
                default: // m
                    HudElements.FromOperator += ((int)Math.Round(cs.DistToHome)).ToString().PadLeft(5); // cs.DistToHome is in meters
                    break;
            }
            HudElements.FromOperator += SettingManager.Get(Setting.DistFormat).ToUpper();

            // Next waypoint distance
            HudElements.ToWaypoint = "WAYPOINT";
            switch (SettingManager.Get(Setting.DistFormat))
            {
                case "km":
                    HudElements.ToWaypoint += (cs.wp_dist * 1000).ToString().PadLeft(5);
                    break;
                case "ft":
                    HudElements.ToWaypoint += (cs.wp_dist * Meter_to_Feet).ToString().PadLeft(5);
                    break;
                default: // m
                    HudElements.ToWaypoint += cs.wp_dist.ToString().PadLeft(5); // cs.wp_dist is in meters
                    break;
            }
            HudElements.ToWaypoint += SettingManager.Get(Setting.DistFormat).ToUpper();
            TimeSpan to_wp = TimeSpan.FromSeconds(cs.tot);
            HudElements.ToWaypoint += $"\n{to_wp.Hours.ToString().PadLeft(2, '0')}:{to_wp.Minutes.ToString().PadLeft(2, '0')}:{to_wp.Seconds.ToString().PadLeft(2, '0')}";

            // Camera angles
            bool hasSysRep = CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport);
            HudElements.Camera = "CAM "
                + "PITCH"
                + (hasSysRep ? (int)Math.Round(((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).pitch) : 0).ToString().PadLeft(5) + "°"
                + "\nYAW"
                + (hasSysRep ? (int)Math.Round(((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).roll) : 0).ToString().PadLeft(7) + "°";

            // UAV position
            (double lat, double lng) droneLatLng = (cs.lat, cs.lng);
            string dronePos;
            if (droneLatLng.lat >= -90 && droneLatLng.lat <= 90
                &&
                droneLatLng.lng >= -180 && droneLatLng.lng <= 180)
            {
                Coordinate droneCoord = new Coordinate(droneLatLng.lat, droneLatLng.lng, DateTime.Now);
                switch (SettingManager.Get(Setting.GPSType).ToUpper())
                {
                    case "MGRS":
                        dronePos = droneCoord.MGRS.ToString();
                        break;
                    default: // WGS84
                        dronePos = droneCoord.UTM.ToString();
                        break;
                }

                CameraHandler.Instance.DronePos = droneCoord; // Update CameraHandler
            }
            else
            {
                dronePos = "UNKNOWN";
            }

            HudElements.DroneGps = "UAV"
                + SettingManager.Get(Setting.GPSType).ToUpper().PadLeft(dronePos.Length - 3)
                + $"\n" + dronePos;

            // Camera target position
            (float lat, float lng) targetLatLng = (hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLat : 0, hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLon : 0);
            string targetPos;
            if (targetLatLng.lat >= -90 && targetLatLng.lat <= 90
                &&
                targetLatLng.lng >= -180 && targetLatLng.lng <= 180)
            {
                Coordinate targetCoord = new Coordinate(targetLatLng.lat, targetLatLng.lng, DateTime.Now);
                switch (SettingManager.Get(Setting.GPSType).ToUpper())
                {
                    case "MGRS":
                        targetPos = targetCoord.MGRS.ToString();
                        break;
                    default: // WGS84
                        targetPos = targetCoord.UTM.ToString();
                        break;
                }

                CameraHandler.Instance.TargPos = targetCoord; // Update CameraHandler
            }
            else
            {
                targetPos = "UNKNOWN";
            }

            HudElements.TargetGps = "TRG"
                + SettingManager.Get(Setting.GPSType).ToUpper().PadLeft(targetPos.Length - 3)
                + $"\n" + targetPos;

            HudElements.LineDistance = 10;
            // TODO: Optimize HudElements.LineDistance on the fly to make it easy to read on the screen

            HudElements.LineSpacing = PixelsForMeters(
                hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsSlantRange : 100.0,
                hasSysRep ? ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).fov : 60.0,
                VideoRectangle.Width, HudElements.LineDistance);

            SetStopButtonVisibility();
        }

        #endregion

        #region EventHandlers

        private void Instance_event_DoPhoto(object sender, DoRecordingEventArgs e)
        {
            this.DoPhoto();
        }

        private void CameraView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if(_videoRecorder != null)
                {
                    _videoRecorder.Stop();
                    _videoRecorder = null;
                }
                
                GC.Collect();

                GStreamer.StopAll();

                _droneStatusTimer.Stop();

                pb_CameraGstream.Paint -= Pb_CameraGstream_Paint;
            }
            catch { }

        }

        private void Pb_CameraGstream_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (img != null)
                {
                    lock (this._bgimagelock)
                    {
                        e.Graphics.DrawImage(img, 0, 0, pb_CameraGstream.Width, pb_CameraGstream.Height);
                    }

                    //FetchHudData();
                    OnNewFrame(img.Width, img.Height, e.Graphics);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void _cameraSwitchOffTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string mode = MainV2.comPort.MAV.cs.mode;
            int agl = (int)MainV2.comPort.MAV.cs.alt;

            bool droneFlightMode = mode.ToUpper() == "GUIDED" || mode.ToUpper() == "AUTO" || mode.ToUpper() == "LOITER";
            bool droneAGLMoreThanZero = agl > 0;
            bool currentStateFlight = StateHandler.CurrentSate == MV04_State.Takeoff || StateHandler.CurrentSate == MV04_State.Follow || StateHandler.CurrentSate == MV04_State.Auto || StateHandler.CurrentSate == MV04_State.Manual;

            int state = 0;

            if (droneAGLMoreThanZero || currentStateFlight)
            {
                return;
            }

            MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_RELAY, CameraHandler.TripChannelNumber, state, 0, 0, 0, 0, 0);
            _tripSwitchedOff = true;
            btn_TripSwitchOnOff.BackColor = Color.Black;
        }

        private void btn_ChangeCrosshair_Click(object sender, EventArgs e)
        {
            ChangeCrossHair();
        }

        private void btn_Up_Click(object sender, EventArgs e)
        {

            if (this.cs_ColorSliderAltitude.Value < _maxAllowedAltitudeValue - 10)
                this.cs_ColorSliderAltitude.Value += 10;
            else
                this.cs_ColorSliderAltitude.Value = _maxAllowedAltitudeValue;

            this.lb_AltitudeValue.Text = cs_ColorSliderAltitude.Value + "m";
        }

        private void btn_Down_Click(object sender, EventArgs e)
        {
            if (this.cs_ColorSliderAltitude.Value > _minAllowedAltitudeValue + 10)
                this.cs_ColorSliderAltitude.Value -= 10;
            else
                this.cs_ColorSliderAltitude.Value = _minAllowedAltitudeValue;

            this.lb_AltitudeValue.Text = cs_ColorSliderAltitude.Value + "m";
        }

        private void btn_FullScreen_Click(object sender, EventArgs e)
        {
            //if (_cameraFullScreenForm != null)
            //{
            //    try
            //    {
            //        _cameraFullScreenForm.Show();
            //    }
            //    catch { }


            //}
            //else
            //{
            //    _cameraFullScreenForm = new CameraFullScreenForm();
            //    _cameraFullScreenForm.VisibleChanged += FullScreenForm_VisibleChanged;
            //    _cameraFullScreenForm.FormClosing += _cameraFullScreenForm_FormClosing;
            //    _cameraFullScreenForm.ShowDialog();
            //}
            _cameraFullScreenForm = new CameraFullScreenForm();
            _cameraFullScreenForm.VisibleChanged += FullScreenForm_VisibleChanged;
            _cameraFullScreenForm.FormClosing += _cameraFullScreenForm_FormClosing;
            _cameraFullScreenForm.ShowDialog();
        }

        private void _cameraFullScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cameraFullScreenForm != null)
            {
                _cameraFullScreenForm.VisibleChanged -= FullScreenForm_VisibleChanged;
                _cameraFullScreenForm.FormClosing -= _cameraFullScreenForm_FormClosing;
                _cameraFullScreenForm.Dispose();
                _cameraFullScreenForm = null;
            }
        }


        private void btn_ResetZoom_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.ResetZoom();
        }

        private void FullScreenForm_VisibleChanged(object sender, EventArgs e)
        {
            ReconnectCameraStreamAndControl();
        }

        private void Form_event_ReconnectRequested(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ReconnectCameraStreamAndControl()));
            }
            else
            {
                ReconnectCameraStreamAndControl();
            }
        }

        private void btn_Settings_Click(object sender, EventArgs e)
        {
            CameraSettingsForm.Instance.ShowDialog();
        }

        private void CameraSettings_event_StartStopRecording(object sender, EventArgs e)
        {
            if(_recordingInProgress)
                StopRecording();
            else
                StartRecording();

            CameraSettingsForm.Instance.SetRecordingStatus(_recordingInProgress);
        }

        private void btn_FPVCameraMode_Click(object sender, EventArgs e)
        {
            if (_isFPVModeActive)
            {
                CameraHandler.Instance.SetMode(CameraHandler.Instance.PrevCameraMode);

                _isFPVModeActive = false;
                btn_FPVCameraMode.BackColor = Color.Black;
            }
            else
            {
                CameraHandler.Instance.SetMode(MavProto.NvSystemModes.Stow);

                _isFPVModeActive = true;
                btn_FPVCameraMode.BackColor = Color.DarkGreen;

            }
        }

        private void btn_StopTracking_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.StopTracking(true);
            IsCameraTrackingModeActive = false;
            SetStopButtonVisibility();
        }

        private void CameraView_Resize(object sender, EventArgs e)
        {
            //if (this.Size.Width < 1270)
            //{
            //    this.pnl_CameraScreen.Padding = new Padding(0, 60, 0, 60);
            //    this.pnl_CameraScreen.MinimumSize = new Size(620, 360);
            //    this.pnl_CameraScreen.Size = new Size(620, 360);
            //}
            //else if (this.Size.Width < 1590)
            //{
            //    this.pnl_CameraScreen.Padding = new Padding(0, 30, 0, 30);
            //    this.pnl_CameraScreen.MinimumSize = new Size(960, 540);
            //    this.pnl_CameraScreen.Size = new Size(960, 540);
            //}
            //else
            //{
            //    this.pnl_CameraScreen.Padding = new Padding(0, 0, 0, 0);
            //    this.pnl_CameraScreen.MinimumSize = new Size(1280, 720);
            //    this.pnl_CameraScreen.Size = new Size(1280, 720);
            //}
        }

        private void CameraHandler_event_ReportArrived(object sender, ReportEventArgs e)
        {
            
            try
            {
                #region test
                byte systemMode = e.Report.systemMode;

                byte test = e.Report.status_flags;

                NvSystemModes stg = CameraHandler.Instance.SysReportModeToMavProtoMode((SysReport)CameraHandler.Instance.CameraReports[MavReportType.SystemReport]);

                _cameraState = stg;
                #endregion

                string systemModeStr = CameraHandler.Instance.SysReportModeToMavProtoMode(e.Report).ToString();

                //Test: Set Camera Status
                if (InvokeRequired)
                    Invoke(new Action(() => { SetCameraStatusValue(systemModeStr); }));
                else
                    SetCameraStatusValue(systemModeStr);

                //Test: Set Drone Status
                if (InvokeRequired)
                    Invoke(new Action(() => { SetDroneStatusValue(); }));
                else
                    SetDroneStatusValue();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }

        private void StateHandler_MV04StateChange(object sender, MV04StateChangeEventArgs e)
        {
            //Test: Set GCS Status
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetGCSStatus()));
            }
            else
                SetGCSStatus();

            if (e.NewState == MV04_State.Follow)
            {
                if(_cameraState != NvSystemModes.Tracking)
                {
                    MessageBox.Show("Camera must tracking before switch to Follow mode! Switch back to the previous set camera Tracking then switch to Follow");
                    Task.Run(() => ProvideGCSError());
                }
                //check van e tracking
            }

            if (e.NewState == MV04_State.Auto)
            {
                
                if (MainV2.comPort.MAV.wps.Values.Count <= 0)
                {
                    MessageBox.Show("No uploaded plan");
                    Task.Run(() => Blink());
                }
                
                
            }
        }

        private void _droneStatustimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { SetDroneStatusValue(); }));
            }
            else
                SetDroneStatusValue();
        }

        private void LEDStateHandler_LedStateChanged(object sender, LEDStateChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { SetDroneLEDStates(e.LandingLEDState, e.PositionLEDState_IR, e.PositionLEDState_RedLight); }));
            }
            else
                SetDroneLEDStates(e.LandingLEDState, e.PositionLEDState_IR, e.PositionLEDState_RedLight);
        }

        private void btn_SetAlt_Click(object sender, EventArgs e)
        {
            var plla = new PointLatLngAlt(MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng, cs_ColorSliderAltitude.Value);

            Locationwp gotohere = new Locationwp();

            gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
            gotohere.alt = (float)plla.Alt / CurrentState.multiplieralt; // back to m
            gotohere.lat = (plla.Lat);
            gotohere.lng = (plla.Lng);

            try
            {
                MainV2.comPort.setGuidedModeWP(gotohere);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
            }
        }

        private void cs_ColorSliderAltitude_ValueChanged(object sender, EventArgs e)
        {
            this.lb_AltitudeValue.Text = cs_ColorSliderAltitude.Value + "m";
        }

        private void btn_TripSwitchOnOff_Click(object sender, EventArgs e)
        {
            if (_tripSwitchedOff)
            {
                MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_RELAY, CameraHandler.TripChannelNumber, 1, 0, 0, 0, 0, 0);
                _tripSwitchedOff = false;

                ReconnectCameraStreamAndControl();
                btn_TripSwitchOnOff.BackColor = Color.DarkGreen;
            }
            else
            {
                MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_RELAY, CameraHandler.TripChannelNumber, 0, 0, 0, 0, 0, 0);
                _tripSwitchedOff = true;
                btn_TripSwitchOnOff.BackColor = Color.Black;
            }


        }

        /// <summary>
        /// Start camera tracking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pb_CameraGstream_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.X <= 0 || e.Y <= 0)
                return;

            if (IsCameraTrackingModeActive)
                return;

            IsCameraTrackingModeActive = true;

            var success = CameraHandler.Instance.StartTracking(new Point(e.X, e.Y));

            //Point _trackPos = new Point(e.X, e.Y);

            //// Constrain tracking pos
            //_trackPos.X = CameraHandler.Instance.Constrain(_trackPos.X, 0, 1280);
            //_trackPos.Y = CameraHandler.Instance.Constrain(_trackPos.Y, 0, 720);

            //MessageBox.Show("(X: " + e.X + " ," + "Y: " + e.Y + ")\n" + "(CX: " + _trackPos.X + ", " + _trackPos.Y + ")");

        }

        #endregion

        #region Methods

        private void SetStopButtonVisibility()
        {
            if (IsCameraTrackingModeActive)
                btn_StopTracking.Visible = true;
            else
                btn_StopTracking.Visible = false;
        }

        private void SetDroneStatusValue()
        {
            string mode = MainV2.comPort.MAV.cs.mode;
            this.lb_DroneStatusValue.Text = mode;

            int agl = (int)MainV2.comPort.MAV.cs.alt;
            this.lb_AltitudeValue.Text = agl.ToString() + "m";
        }

        private void SetCameraStatusValue(string st)
        {
            this.lb_CameraStatusValue.Text = st;
        }

        private void SetGCSStatus()
        {
            this.lb_GCSSelectedStateValue.Text = StateHandler.CurrentSate.ToString();

            CheckStatus();
        }

        /// <summary>
        /// Do blinking and error (provider or MSGBox) if states are inconsistent
        /// </summary>
        private void CheckStatus()
        {
            if (!CameraHandler.Instance.HasCameraReport(MavReportType.SystemReport) /* || MainV2.comPort.MAV.cs.mode ==  ||*/)
            {
                Task.Run(() => ProvideCameraError());
                MessageBox.Show("Camera no communication");
                return;
            }

            NvSystemModes cameraMode = CameraHandler.Instance.SysReportModeToMavProtoMode((SysReport)CameraHandler.Instance.CameraReports[MavReportType.SystemReport]);

            switch (StateHandler.CurrentSate)
            {
                case MV04_State.Manual:
                    if (cameraMode != NvSystemModes.GRR)
                    {
                        Task.Run(() => ProvideCameraError());
                    }
                    if(  MainV2.comPort.MAV.cs.mode.ToUpper() != "LOITER")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.TapToFly:
                    if (cameraMode != NvSystemModes.GRR)
                    {
                        Task.Run(() => ProvideCameraError());
                    }
                    if (MainV2.comPort.MAV.cs.mode.ToUpper() != "AUTO")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.Auto:
                    if (cameraMode != NvSystemModes.GRR)
                    {
                        Task.Run(() => ProvideCameraError());
                    }
                    if (MainV2.comPort.MAV.cs.mode.ToUpper() != "AUTO")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.Follow:
                    if (cameraMode != NvSystemModes.Tracking)
                    {
                        Task.Run(() => ProvideCameraError());
                    }
                    if (MainV2.comPort.MAV.cs.mode.ToUpper() != "FOLLOW")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.FPV:
                    if (cameraMode != NvSystemModes.Stow)
                    {
                        Task.Run(() => ProvideCameraError());
                    }
                    //if (MainV2.comPort.MAV.cs.mode.ToUpper() != "LOITER")
                    //{
                    //    Task.Run(() => ProvideDroneError());
                    //}
                    break;
                case MV04_State.Takeoff:
                    break;
                case MV04_State.RTL:
                    break;
                case MV04_State.Land:
                    break;
                case MV04_State.Unknown:
                    break;
                default: 
                    break;

            }



        }

        private const int MAXBLINK = 14;

        private async void Blink()
        {
            int _blinkCounter = 0;
            while (_blinkCounter != MAXBLINK)
            {
                await Task.Delay(500);
                tlp_DeviceStatusInfo.BackColor = tlp_DeviceStatusInfo.BackColor == Color.Red ? Color.Black : Color.Red;
                ++_blinkCounter;
            }
        }

        private async void BlinkControl(Control control)
        {
            if (control == null)
                return;

            int _blinkCounter = 0;
            while (_blinkCounter != MAXBLINK)
            {
                await Task.Delay(500);
                tlp_DeviceStatusInfo.BackColor = control.BackColor == Color.Red ? Color.Black : Color.Red;
                ++_blinkCounter;
            }
        }

        private void ProvideCameraError()
        {
            BlinkControl(this.pnl_Camera);
        }

        private void ProvideDroneError()
        {
            BlinkControl(this.pnl_Drone);
        }

        private void ProvideGCSError()
        {
            BlinkControl(this.pnl_GCS);
        }

        private void SetDroneLEDStates(enum_LandingLEDState landing, enum_PositionLEDState_IR position_IR, enum_PositionLEDState_RedLight position_LEDLight)
        {
            //LED
            if (landing == enum_LandingLEDState.On)
                this.pb_DroneTakeOff.Visible = true;
            else
                this.pb_DroneTakeOff.Visible = false;
            //IR
            if(position_IR == enum_PositionLEDState_IR.On)
                this.pb_InfraLight.Visible = true;
            else
                this.pb_InfraLight.Visible = false;
            //Red
            if (position_LEDLight == enum_PositionLEDState_RedLight.On)
                this.pb_PositionIndicator.Visible = true;
            else
                this.pb_PositionIndicator.Visible = false;


        }

        #endregion
    }
}
