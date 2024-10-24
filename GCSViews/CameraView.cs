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
using MV04.FlightPlanAnalyzer;
using System.Timers;
using MissionPlanner.Controls;
using LibVLCSharp.Shared;

namespace MissionPlanner.GCSViews
{
    public partial class CameraView : MyUserControl//, IActivate, IDeactivate
    {
        #region Fields

        public static CameraView instance;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        string CameraIP;
        int CameraStreamChannel;
        HudElements HudElements = new HudElements();

        (int major, int minor, int build) CameraControlDLLVersion;

        bool OSDDebug = true;
        string[] OSDDebugLines = new string[10];

        private CameraFullScreenForm _cameraFullScreenForm;
        private bool _isFPVModeActive = false;
        public static bool IsCameraTrackingModeActive { get; set; } = false;

        System.Timers.Timer _droneStatusTimer;
        System.Timers.Timer _cameraSwitchOffTimer;

        public const int _maxAllowedAltitudeValue = 500;
        public const int _minAllowedAltitudeValue = 50;

        private bool _tripSwitchedOff = !bool.Parse(SettingManager.Get(Setting.AutoConnect));
        public bool TripSwitchedOff
        {
            get { return _tripSwitchedOff; }
        }

        Bitmap _actualCameraImage;

        bool _recordingInProgress = false;
        int _segmentLength;

        string _tempPath = "";
        int _fileCount = 0;
        NvSystemModes _cameraState;

        public float SlantRange
        {
            get
            {
                float pitch = ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).pitch;
                float height = MainV2.comPort.MAV.cs.alt;

                float result = (float)(height / Math.Cos(pitch));

                return result;
            }

        }

        System.Timers.Timer _videoRecordSegmentTimer = new System.Timers.Timer();

        private static LibVLCSharp.Shared.LibVLC _libVlc;
        private static MediaPlayer _mediaPlayer;
        private static Media media;
        private static string videoUrl = "rtp://225.1.2.3:11024/live0";

        LibVLCSharp.Shared.LibVLC _vlcRecord;
        MediaPlayer _mediaPlayerRecord;
        Media _mediaRecor;

        Panel panelDoubleClick;
        Label lb_FollowDebugText;

        System.Timers.Timer _feedTimer = new System.Timers.Timer();
        ElapsedEventHandler handler = null;
        double followTestCounter = 0.0;

        private object lckStart = new object();
        bool _enableCrossHair = true;
        private const int MAXBLINK = 14;

        #endregion

        #region Conversion multipliers
        const double Meter_to_Feet = 3.2808399;
        const double Mps_to_Kmph = 3.6;
        const double Mps_to_Knots = 1.94384449;
        #endregion

        #region Constructor

        public CameraView()
        {
            log.Info("Constructor");
            InitializeComponent();
            instance = this;

            #region Camera set up

            CameraIP = SettingManager.Get(Setting.CameraIP);
            CameraStreamChannel = int.Parse(SettingManager.Get(Setting.CameraStreamChannel));
            CameraControlDLLVersion = CameraHandler.Instance.CameraControlDLLVersion;

            // Snapshot & video save location
            CameraHandler.Instance.MediaSavePath = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_media" + Path.DirectorySeparatorChar;


            // SysID for camera functions
            CameraHandler.sysID = MainV2.comPort.sysidcurrent;
            MainV2.comPort.MavChanged += (sender, eventArgs) => CameraHandler.sysID = MainV2.comPort.sysidcurrent; // Update sysID on new connection

            StartCameraStream();

            StartCameraControl();

            CameraHandler.Instance.event_ReportArrived += CameraHandler_event_ReportArrived;
            CameraHandler.Instance.event_DoPhoto += Instance_event_DoPhoto;

            CameraHandler.Instance.SetEnableCrossHair(_enableCrossHair);

            #endregion

            #region UI config


            // Draw UI
            DrawUI();
            DisableControls();
            SetStopButtonVisibility();
            SetSingleYawButton();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            this.DoubleBuffered = true;

            #endregion

            #region State handling

            SetDroneStatusValue();
            SetDroneLEDStates(enum_LandingLEDState.Off, enum_PositionLEDState_IR.Off, enum_PositionLEDState_RedLight.Off);
            LEDStateHandler.LedStateChanged += LEDStateHandler_LedStateChanged;

            StateHandler.MV04StateChange += StateHandler_MV04StateChange;

            if (!CameraHandler.Instance.HasCameraReport(MavReportType.SystemReport))
                this.lb_CameraStatusValue.Text = "NoCom";

            _droneStatusTimer = new System.Timers.Timer();
            _droneStatusTimer.Elapsed += _droneStatustimer_Elapsed;
            _droneStatusTimer.Interval = 3000;
            _droneStatusTimer.Enabled = true;

            #endregion

            #region altitude control set up 

            if((int)MainV2.comPort.MAV.cs.alt < _minAllowedAltitudeValue)
                this.cs_ColorSliderAltitude.Value = _minAllowedAltitudeValue;
            else if((int)MainV2.comPort.MAV.cs.alt > _maxAllowedAltitudeValue)
                this.cs_ColorSliderAltitude.Value -= _maxAllowedAltitudeValue;
            else
                this.cs_ColorSliderAltitude.Value = (int)MainV2.comPort.MAV.cs.alt;

            #endregion

            #region auto TRIP switch off

            //timer for camera switchoff
            _cameraSwitchOffTimer = new System.Timers.Timer();
            _cameraSwitchOffTimer.Elapsed += _cameraSwitchOffTimer_Elapsed;
            _cameraSwitchOffTimer.Interval = 5*60000;
            _cameraSwitchOffTimer.Enabled = true;
            SetTripOnOffButton();

            #endregion

            #region debug label

            lb_FollowDebugText = new Label();
            lb_FollowDebugText.Text = "Debug";
            lb_FollowDebugText.ForeColor = Color.White;
            lb_FollowDebugText.Font = new Font("Stencil", 14);
            this.Location = new Point(10, (this.Height / 3));
            this.Controls.Add(lb_FollowDebugText);
            lb_FollowDebugText.BringToFront();

            #endregion

            #region recording vlc stream

            _segmentLength = int.Parse(SettingManager.Get(Setting.VideoSegmentLength));
            _videoRecordSegmentTimer.Interval = _segmentLength * 1000;
            _videoRecordSegmentTimer.Elapsed += _videoRecordSegmentTimer_Tick;

            if (bool.Parse(SettingManager.Get(Setting.AutoRecordVideoStream)))
            {
                StartRecording();
            }

            #endregion

            this.FormClosing += CameraView_FormClosing;
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

        private void DrawUI()
        {
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
                {"Feed telemetry", () => { StartFeed(); }},
                {"Stop Feed telemetry", () => { StopFeed(); }},
                {"Check Flightplan", async () => { MainV2.CheckFlightPlan(null, new MV04StateChangeEventArgs(){PreviousState = MV04_State.Manual, NewState = MV04_State.Auto}); }}
            };

            #endregion

            #region Dev debug tools

#if DEBUG

            ComboBox cb_TestFunctions = new ComboBox();
            cb_TestFunctions.DropDownStyle = ComboBoxStyle.DropDownList;
            cb_TestFunctions.Items.AddRange(testFunctions.Keys.ToArray());
            cb_TestFunctions.SelectedIndex = 0;
            cb_TestFunctions.Location = new Point(10, (this.Height / 3) + 0);
            cb_TestFunctions.Width = 300;
            cb_TestFunctions.Font = new Font("Stencil", 16);
            this.Controls.Add(cb_TestFunctions);
            cb_TestFunctions.BringToFront();

            Button bt_DoTestFunction = new Button();
            bt_DoTestFunction.Text = "Test function";
            bt_DoTestFunction.Location = new Point(10, (this.Height / 3) + 25);
            bt_DoTestFunction.Width = 300;
            bt_DoTestFunction.Height = 50;
            bt_DoTestFunction.BackColor = Color.White;
            bt_DoTestFunction.ForeColor = Color.Black;
            bt_DoTestFunction.Click += (sender, e) => testFunctions[cb_TestFunctions.SelectedItem.ToString()]();
            this.Controls.Add(bt_DoTestFunction);
            bt_DoTestFunction.BringToFront();
#endif

            #endregion
        }

        #endregion

        private void StartFeed()
        {
            PointLatLngAlt target = new PointLatLngAlt();
            _feedTimer.Interval = 10000;
            _feedTimer.Elapsed += handler = new ElapsedEventHandler(delegate (object sender, ElapsedEventArgs e) {

                if (CameraView.instance.IsDisposed)
                    return;

                /*
                    Simulated copter desired positions
                    -35.3621410612455	149.164499044418	1
                    -35.3619048250147	149.166129827499	2

                 */

                if (!IsCameraTrackingModeActive)
                {
                    //camera not tracking 
                }

                if (CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.GndCrsReport))
                {
                    var target_lat = ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLat;
                    var target_lng = ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLon;
                    var target_alt = ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsAlt;

#if DEBUG

                    //debug to screen
                    if (InvokeRequired)
                        Invoke(new Action(() => lb_FollowDebugText.Text = "Target pos -> lat: " + target_lat + " lng: " + target_lng + " alt: " + target_alt));
                    else
                        lb_FollowDebugText.Text = "Target pos -> lat: " + target_lat + " lng: " + target_lng + " alt: " + target_alt;

#endif

                    MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
                    {
                        alt = target_alt + 30,
                        lat = target_lat, //+ followTestCounter,                    
                        lng = target_lng,
                        id = (ushort)MAVLink.MAV_CMD.WAYPOINT
                    });
                    //followTestCounter += 0.001;
                }
                else
                {
                    //no report from camera
                }


            });

            _feedTimer.Start();
        }

        private void StopFeed()
        {
            _feedTimer.Stop();
        }

        #region CameraFunctions

        private void StartCameraStream()
        {
            StartVideoStreamVLC();
        }

        private void StartCameraControl()
        {
            bool success;
            lock (lckStart)
            {
                success = CameraHandler.Instance.CameraControlConnect(
                IPAddress.Parse(SettingManager.Get(Setting.CameraIP)),
                int.Parse(SettingManager.Get(Setting.CameraControlPort)));

                bool autoStartSingleYaw = bool.Parse(SettingManager.Get(Setting.AutoStartSingleYaw));

                // Auto start single-yaw loop
                if (success && autoStartSingleYaw)
                {
                    SingleYawHandler.StartSingleYaw(MainV2.comPort);

                    if (InvokeRequired)
                        Invoke(new Action(() => SetSingleYawButton()));
                    else
                        SetSingleYawButton();
                }
            }
            

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
            CameraHandler.Instance.SetEnableCrossHair(!_enableCrossHair);
            _enableCrossHair = !_enableCrossHair;
        }

        #endregion

        private void DoPhoto(string path = null)
        {
            _actualCameraImage = new Bitmap(vv_VLC.TopLevelControl.Width, vv_VLC.TopLevelControl.Height);
            vv_VLC.TopLevelControl.DrawToBitmap(_actualCameraImage, new Rectangle(0, 0, vv_VLC.TopLevelControl.Width, vv_VLC.TopLevelControl.Height));

            if (path == null)
                path = CameraHandler.Instance.MediaSavePath + "test" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";

            if (_actualCameraImage != null)
                _actualCameraImage.Save(path, ImageFormat.Jpeg);

#if DEBUG
            AddToOSDDebug("Photo taken");
#endif
        }

        private void _videoRecordSegmentTimer_Tick(object sender, EventArgs e)
        {
            _mediaPlayerRecord.Stop();
            _mediaPlayerRecord.Play(_mediaRecor);
        }

        private void StartRecording()
        {
            if(_vlcRecord == null)
                _vlcRecord = new LibVLCSharp.Shared.LibVLC();

            if(_mediaPlayerRecord == null)
                _mediaPlayerRecord = new MediaPlayer(_vlcRecord);

            if(_mediaRecor == null)
            {
                _mediaRecor = new Media(_vlcRecord, new Uri(videoUrl));
                _mediaRecor.AddOption(":sout=#file{dst=" + CameraHandler.Instance.MediaSavePath + "savetestTTTT" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".ts" + "}");
            }

            _mediaPlayerRecord.Play(_mediaRecor);
            _videoRecordSegmentTimer?.Start();
            _recordingInProgress = true;
        }

        public void StopRecording()
        {
            _videoRecordSegmentTimer?.Stop();

            if(_mediaPlayerRecord != null)
                _mediaPlayerRecord.Stop();

            _recordingInProgress = false;
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

        #endregion

        #region Drawing

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

        #region EventHandlers

        private void Instance_event_DoPhoto(object sender, DoRecordingEventArgs e)
        {
            this.DoPhoto();
        }

        private void CameraView_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _droneStatusTimer.Elapsed -= _droneStatustimer_Elapsed;
                CameraHandler.Instance.event_ReportArrived -= CameraHandler_event_ReportArrived;
                CameraHandler.Instance.event_DoPhoto -= Instance_event_DoPhoto;

                _droneStatusTimer.Dispose();
                _cameraSwitchOffTimer.Dispose();
                _feedTimer.Dispose();
                _videoRecordSegmentTimer.Dispose();

                GC.Collect();

                this.Dispose();
            }
            catch { }

        }

        private void _cameraSwitchOffTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CameraView.instance.IsDisposed)
                return;

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
            SetTripOnOffButton();
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

        /// <summary>
        /// Stop camera tracking and set flag to false and set btn_Stop visibility false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_StopTracking_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.StopTracking(true);
            IsCameraTrackingModeActive = false;
            SetStopButtonVisibility();
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
            if (CameraView.instance.IsDisposed)
                return;

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
            if (CameraView.instance.IsDisposed)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action(() => { SetDroneStatusValue(); }));
            }
            else
                SetDroneStatusValue();
        }

        private void LEDStateHandler_LedStateChanged(object sender, LEDStateChangedEventArgs e)
        {
            if (CameraView.instance.IsDisposed)
                return;

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
                SwitchOnTrip();
                //ReconnectCameraStreamAndControl();
            }
            else
            {
                MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_RELAY, CameraHandler.TripChannelNumber, 0, 0, 0, 0, 0, 0);
                _tripSwitchedOff = true;
                SetTripOnOffButton();
            }
        }

        public void SwitchOnTrip()
        {
            
            MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_SET_RELAY, CameraHandler.TripChannelNumber, 1, 0, 0, 0, 0, 0);
            _tripSwitchedOff = false;

            SetTripOnOffButton();
        }

        #endregion

        #region Methods

        private void SetStopButtonVisibility()
        {
            if (!CameraView.instance.IsDisposed)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => {
                        if (IsCameraTrackingModeActive)
                            btn_StopTracking.Visible = true;
                        else
                            btn_StopTracking.Visible = false;
                    }));
                }
                else
                {
                    if (IsCameraTrackingModeActive)
                        btn_StopTracking.Visible = true;
                    else
                        btn_StopTracking.Visible = false;
                }
            }
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

        private void btn_StartStopSingleYaw_Click(object sender, EventArgs e)
        {
            if (SingleYawHandler.IsRunning)
            {
                SingleYawHandler.StopSingleYaw();
                this.btn_StartStopSingleYaw.Text = "Start Single Yaw";
                this.btn_StartStopSingleYaw.BackColor = Color.Black;
            }
            else
            {
                SingleYawHandler.StartSingleYaw(MainV2.comPort);
                if (SingleYawHandler.IsRunning)
                {
                    this.btn_StartStopSingleYaw.Text = "Stop Single Yaw";
                    this.btn_StartStopSingleYaw.BackColor = Color.DarkGreen;
                }
            }
        }

        private void SetSingleYawButton()
        {
            if (!SingleYawHandler.IsRunning)
            {
                this.btn_StartStopSingleYaw.Text = "Start Single Yaw";
                this.btn_StartStopSingleYaw.BackColor = Color.Black;
            }
            else
            {
                this.btn_StartStopSingleYaw.Text = "Stop Single Yaw";
                this.btn_StartStopSingleYaw.BackColor = Color.DarkGreen;
            }
        }

        private void CameraView_VisibleChanged(object sender, EventArgs e)
        {
            SetSingleYawButton();
        }

        private void vv_VLC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.X <= 0 || e.Y <= 0)
                return;

            IsCameraTrackingModeActive = true;

            var success = CameraHandler.Instance.StartTracking(new Point(e.X, e.Y), this.vv_VLC.Size);

            SetStopButtonVisibility();
        }

        private void StartVLC()
        {
            _libVlc = new LibVLCSharp.Shared.LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);
            media = new Media(_libVlc, new Uri(videoUrl));

            
            _mediaPlayer.EnableHardwareDecoding = true;
            _mediaPlayer.NetworkCaching = 300;

            vv_VLC.MediaPlayer = _mediaPlayer;
            this.tlp_CVBase.Controls.Add(this.vv_VLC, 0, 0);
            vv_VLC.Dock = DockStyle.Fill;
            _mediaPlayer.Fullscreen = true;

            if (panelDoubleClick == null)
            {
                panelDoubleClick = new Panel();// panel for double click
                vv_VLC.Controls.Add(panelDoubleClick);
                panelDoubleClick.BringToFront();
                panelDoubleClick.Dock = DockStyle.Fill;
                panelDoubleClick.BackColor = Color.Transparent;
                panelDoubleClick.MouseDoubleClick += new MouseEventHandler(vv_VLC_MouseDoubleClick);
            }
            else
                panelDoubleClick.MouseDoubleClick += new MouseEventHandler(vv_VLC_MouseDoubleClick);

            
            vv_VLC.ThisReallyVisible();
            vv_VLC.ChildReallyVisible();
            _mediaPlayer.Play(media);

        }

        public void StartVideoStreamVLC()
        {
            if (InvokeRequired)
                Invoke(new Action(() => StartVLC()));
            else
                StartVLC();
        }

        public void StopVLC()
        {
            if (InvokeRequired)
                Invoke(new Action(() => {
                    _mediaPlayer.Stop();
                    panelDoubleClick.MouseDoubleClick -= new MouseEventHandler(vv_VLC_MouseDoubleClick);
                }));
            else
            {
                _mediaPlayer.Stop();
                panelDoubleClick.MouseDoubleClick -= new MouseEventHandler(vv_VLC_MouseDoubleClick);
            }
        }

        private void SetTripOnOffButton()
        {

            if (InvokeRequired)
                Invoke(new Action(() => {
                    if (_tripSwitchedOff)
                    {
                        btn_TripSwitchOnOff.BackColor = Color.Black;
                        btn_TripSwitchOnOff.Text = "Trip is Off";
                    }

                    else
                    {
                        btn_TripSwitchOnOff.BackColor = Color.DarkGreen;
                        btn_TripSwitchOnOff.Text = "Trip is On";
                    }
                }));
            else
            {
                if (_tripSwitchedOff)
                {
                    btn_TripSwitchOnOff.BackColor = Color.Black;
                    btn_TripSwitchOnOff.Text = "Trip is Off";
                }

                else
                {
                    btn_TripSwitchOnOff.BackColor = Color.DarkGreen;
                    btn_TripSwitchOnOff.Text = "Trip is On";
                }
            }
        }

    }
}
