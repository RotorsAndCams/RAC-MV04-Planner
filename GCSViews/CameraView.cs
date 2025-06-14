﻿using CoordinateSharp;
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
using MV04.FlightPlanAnalyzer;
using System.Timers;
using MissionPlanner.Controls;
using LibVLCSharp.Shared;
using Accord.MachineLearning.VectorMachines.Learning;
using static MAVLink;

namespace MissionPlanner.GCSViews
{
    public partial class CameraView : MyUserControl//, IActivate, IDeactivate
    {
        #region Fields

        public static CameraView instance;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        string CameraIP;
        HudElements HudElements = new HudElements();

        (int major, int minor, int build) CameraControlDLLVersion;

        bool OSDDebug = true;
        string[] OSDDebugLines = new string[10];

        private bool _isFPVModeActive = false;
        public static bool IsCameraTrackingModeActive { get; set; } = false;

        System.Timers.Timer _droneStatusTimer;

        public const int _maxAllowedAltitudeValue = 500;
        public const int _minAllowedAltitudeValue = 50;

        Bitmap _actualCameraImage;

        bool _recordingInProgress = false;
        int _segmentLength;

        string _tempPath = "";
        int _fileCount = 0;
        NvSystemModes _cameraState = NvSystemModes.Stow;
        bool isPolarityInverted = false;

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

        LibVLCSharp.Shared.LibVLC _vlcRecord;
        MediaPlayer _mediaPlayerRecord;
        Media _mediaRecord;

        Panel panelDoubleClick;

        System.Timers.Timer _feedTimer;
        ElapsedEventHandler handler = null;

        private object lckStart = new object();
        bool _enableCrossHair = true;
        private const int MAXBLINK = 10;
        bool _needToResetTime = false;

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

            CameraControlDLLVersion = CameraHandler.Instance.CameraControlDLLVersion;
            CameraIP = SettingManager.Get(Setting.CameraIP);

            StartCameraStream();
            StartCameraControl();

            // Snapshot & video save location
            CameraHandler.Instance.MediaSavePath = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_media" + Path.DirectorySeparatorChar;

            // SysID for camera functions
            CameraHandler.sysID = MainV2.comPort.sysidcurrent;

            if(MainV2.comPort != null)
                MainV2.comPort.MavChanged += (sender, eventArgs) => CameraHandler.sysID = MainV2.comPort.sysidcurrent; // Update sysID on new connection

            CameraHandler.Instance.event_ReportArrived += CameraHandler_event_ReportArrived;
            CameraHandler.Instance.event_DoPhoto += Instance_event_DoPhoto;

            CameraHandler.Instance.SetEnableCrossHair(_enableCrossHair);

            #endregion

            #region UI config

            // Draw UI
            if(MainV2.instance.devmode)
                DrawUI();

            SetStopButtonVisibility();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            this.DoubleBuffered = true;

            MainV2.instance.RelaySwitched += MainV2_RelaySwitched;

            #endregion

            #region State handling

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

            #region recording vlc stream
            //hol az autostart record?

            _segmentLength = int.Parse(SettingManager.Get(Setting.VideoSegmentLength));

            if (_segmentLength < 60)
                _segmentLength = 60;

            _videoRecordSegmentTimer.Interval = _segmentLength * 1000;
            _videoRecordSegmentTimer.Elapsed += _videoRecordSegmentTimer_Tick;

            #endregion

            #region Follow mode

            _feedTimer = new System.Timers.Timer();
            _feedTimer.Interval = 1000 * 5;
            _feedTimer.Elapsed += _feedTimer_Elapsed;
            _feedTimer.Enabled = false;

            #endregion

            columnWidth = tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width;

            this.FormClosing += CameraView_FormClosing;

            SetDroneStatusValue();

            MainV2.comPort.CommsClose += ComPort_CommsClose;

            if(bool.Parse(SettingManager.Get(Setting.AutoConnect)))
                this.btn_TripSwitchOnOff.Enabled = false;
        }

        private void ComPort_CommsClose(object sender, EventArgs e)
        {
            //stop recording
            StopRecording();

        }

        public bool isCameraConnected = false;
        public bool controlsClosed = false;
        float columnWidth;
        PictureBox btn = new PictureBox();

        public void SetMenu()
        {
            if (controlsClosed)
            {
                ShowMenu();
            }
            else
            {
                HideMenu();
            }
        }

        public void HideMenu()
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width = 0;
                    controlsClosed = true;
                    btn.Text = "Open";
                }));
            else
            {
                tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width = 0;
                controlsClosed = true;
                btn.Text = "Open";
            }
        }

        public void ShowMenu()
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width = columnWidth;
                    vv_VLC.Dock = DockStyle.Fill;
                    vv_VLC.BringToFront();
                    controlsClosed = false;
                    btn.Text = "Close";
                    MainV2.instance.HideflightData();
                }));
            else
            {
                tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width = columnWidth;
                vv_VLC.Dock = DockStyle.Fill;
                vv_VLC.BringToFront();
                controlsClosed = false;
                btn.Text = "Close";
                MainV2.instance.HideflightData();
            }
        }

        public void ResetMenuCollapse()
        {
            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width = columnWidth;
                    vv_VLC.Dock = DockStyle.Fill;
                    vv_VLC.BringToFront();
                    controlsClosed = false;
                    btn.Text = "Close";
                }));
            else
            {
                tlp_CVBase.ColumnStyles[tlp_CVBase.ColumnStyles.Count - 1].Width = columnWidth;
                vv_VLC.Dock = DockStyle.Fill;
                vv_VLC.BringToFront();
                controlsClosed = false;
                btn.Text = "Close";
            }
        }

        #endregion

        #region Init

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.A))
            {
                //to stg
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
                {"Start recording", () => { StartRecording(); }},
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
                {"Feed telemetry", () => { StartFeed(); }},
                {"Stop Feed telemetry", () => { StopFeed(); }},
                {"Sync Time", () => { CameraHandler.Instance.SetSystemTimeToCurrent(); }},
                {"Set Waypoint", () => {  new Thread(() => new SetWaypointForm().ShowDialog()).Start();   /*new SetWaypointForm().Show();*/  }},
                {"Check Flightplan", async () => { MainV2.CheckFlightPlan(null, new MV04StateChangeEventArgs(){PreviousState = MV04_State.Manual, NewState = MV04_State.Auto}); }},
                {"Set Stream URL", () => { ShowStreamUrlForm(); }}
            };

            #endregion

            #region Dev debug tools

#if DEBUG
            if (MainV2.instance.devmode)
            {
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
            }
#endif

            #endregion
        }

        #endregion

        private void ShowStreamUrlForm()
        {
            StreamUrlForm frm = new StreamUrlForm(SettingManager.Get(Setting.CameraStreamUrl));
            frm.Show();
        }

        public void SetNewURL(string url)
        {
            // Set new URL
            SettingManager.Set(Setting.CameraStreamUrl, url);
            SettingManager.Save();
            log.Info("Stream URL set to: " + url);

            // Restart VLC
            CameraView.instance.StopVLC();
            Thread.Sleep(2000);
            CameraView.instance.StartVideoStreamVLC();

            // Check if URL was set
            if (media.Mrl == SettingManager.Get(Setting.CameraStreamUrl))
            {
                MessageBox.Show("Stream URL a következőre lett állítva: " + url);
            }
            else
            {
                MessageBox.Show("Stream URL beállítás sikertelen");
            }
        }

        private void StartFeed()
        {
            MainV2.comPort.setMode((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, "GUIDED");
            _feedTimer.Enabled = true;
            _feedTimer.Start();
        }

        bool followAltModified = false;
        private void _feedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Check track mode
            if (!IsCameraTrackingModeActive || _cameraState != NvSystemModes.Tracking)
            {
                CustomMessageBox.Show("Camera nincs tracking üzemmódban");
                StopFeed();
                return;
            }

            if (!MainV2.comPort.MAV.cs.connected || MainV2.comPort.MAV.cs.failsafe)
            {
                StopFeed();
                CustomMessageBox.Show("Nincs kapcsolat a követés leállt");
                return;
            }

            if (CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.GndCrsReport))
            {
                float target_lat = ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLat;
                float target_lng = ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLon;
                float target_alt = (int)MainV2.comPort.MAV.cs.alt;

                //MainV2.comPort.MAV.GuidedMode.z = target_alt / CurrentState.multiplieralt;

                if (MainV2.comPort.MAV.GuidedMode.z < 50)
                    MainV2.comPort.MAV.GuidedMode.z = 50 / CurrentState.multiplieralt;

                #region Follow alt set to max alt fence limitation

                try
                {
                    double fenceAltMaxValue = MainV2.comPort.MAV.param["FENCE_ALT_MAX"].Value;
                    if (fenceAltMaxValue < MainV2.comPort.MAV.GuidedMode.z)
                    {
                        StopFeed();
                        CustomMessageBox.Show("Fence magasság kisebb mint a követési magasság - a követés leállt");
                    }
                }
                catch
                {
                    StopFeed();
                    CustomMessageBox.Show("Hiba - a követés leállt");
                    return;
                }

                #endregion

                MainV2.comPort.MAV.GuidedMode.command = (byte)MAV_CMD.WAYPOINT;
                MainV2.comPort.MAV.GuidedMode.x = (int)((target_lat - 0.0002) * 1e7);
                MainV2.comPort.MAV.GuidedMode.y = (int)((target_lng - 0.0002) * 1e7);

                try
                {
                    //MainV2.comPort.ShowInfo = true;
                    MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
                    {
                        alt = MainV2.comPort.MAV.GuidedMode.z,
                        lat = MainV2.comPort.MAV.GuidedMode.x / 1e7,
                        lng = MainV2.comPort.MAV.GuidedMode.y / 1e7,
                        id = (ushort)MAVLink.MAV_CMD.WAYPOINT
                    });

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
                }
            }
        }

        private void StopFeed()
        {
            _feedTimer.Stop();
            _feedTimer.Enabled = false;
            _feedTimer.Close();
        }

        #region CameraFunctions

        private void StartCameraStream()
        {
            StartVideoStreamVLC();
        }

        public void StartCameraControl()
        {
            bool success;
            lock (lckStart)
            {
                success = CameraHandler.Instance.CameraControlConnect(
                IPAddress.Parse(SettingManager.Get(Setting.CameraIP)),
                int.Parse(SettingManager.Get(Setting.CameraControlPort)));

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
            Task.Factory.StartNew(() => {
                if (InvokeRequired)
                    Invoke(new Action(() => {
                        //StartCameraStream();
                        StartCameraControl();
                        CameraHandler.Instance.SetSystemTimeToCurrent();
                    }));
                else
                {
                    //StartCameraStream();
                    StartCameraControl();
                    CameraHandler.Instance.SetSystemTimeToCurrent();
                }
            });
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
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                bitmap.Save(CameraHandler.Instance.MediaSavePath + "SnapShot" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg", ImageFormat.Jpeg);
            }

            _mediaPlayer.TakeSnapshot(0, CameraHandler.Instance.MediaSavePath + "VideoStreamSnapShot" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg", (uint)vv_VLC.Bounds.Width, (uint)vv_VLC.Bounds.Height);
        }

        private void _videoRecordSegmentTimer_Tick(object sender, EventArgs e)
        {
            _mediaPlayerRecord.Stop();
            StartRecording();
        }

        private void StartRecording()
        {
            try
            {
                if (isCameraConnected)
                {
                    if (_vlcRecord == null)
                        _vlcRecord = new LibVLCSharp.Shared.LibVLC();

                    if (_mediaPlayerRecord == null)
                        _mediaPlayerRecord = new MediaPlayer(_vlcRecord);

                    if (_mediaRecord == null)
                    {
                        _mediaRecord = new Media(_vlcRecord, new Uri(SettingManager.Get(Setting.CameraStreamUrl)));
                        _mediaRecord.AddOption(":sout=#transcode{vcodec=mp4v,acodec=none,vb=128,deinterlace}:std{access=file,mux=mp4,dst=" + CameraHandler.Instance.MediaSavePath + "streamRecord" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4" + "}");
                    }
                    else
                    {
                        _mediaRecord = null;
                        _mediaRecord = new Media(_vlcRecord, new Uri(SettingManager.Get(Setting.CameraStreamUrl)));
                        _mediaRecord.AddOption(":sout=#transcode{vcodec=mp4v,acodec=none,vb=128,deinterlace}:std{access=file,mux=mp4,dst=" + CameraHandler.Instance.MediaSavePath + "streamRecord" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4" + "}");
                    }

                    _mediaPlayerRecord.Play(_mediaRecord);
                    _videoRecordSegmentTimer?.Start();
                    _recordingInProgress = true;
                }
            }
            catch
            {
                CustomMessageBox.Show("Videó rögzítése sikertelen");
            }
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
                StopRecording();
                StopVLC();
                _droneStatusTimer.Elapsed -= _droneStatustimer_Elapsed;
                CameraHandler.Instance.event_ReportArrived -= CameraHandler_event_ReportArrived;
                CameraHandler.Instance.event_DoPhoto -= Instance_event_DoPhoto;

                _droneStatusTimer.Dispose();
                _feedTimer.Dispose();
                _videoRecordSegmentTimer.Dispose();

                GC.Collect();

                this.Dispose();
            }
            catch { }

        }

        private void btn_ChangeCrosshair_Click(object sender, EventArgs e)
        {
            ChangeCrossHair();
        }

        FormNoTheme _fsForm;

        private void btn_FullScreen_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fsForm == null)
                {
                    // Media player
                    this.tlp_CVBase.Controls.Remove(this.vv_VLC);
                    _fsForm = new FormNoTheme();
                    _fsForm.Controls.Add(this.vv_VLC);
                    vv_VLC.Dock = DockStyle.Fill;
                    vv_VLC.BringToFront();
                    _mediaPlayer.Fullscreen = true;
                    _fsForm.WindowState = FormWindowState.Maximized;
                    _fsForm.FormClosing += _fsForm_FormClosing;

                    // Buttons
                    ButtonNoTheme zoomResetButton = new ButtonNoTheme();
                    zoomResetButton.Name = "zoomResetButton";
                    zoomResetButton.Text = btn_ResetZoom.Text;
                    zoomResetButton.Font = btn_ResetZoom.Font;
                    zoomResetButton.Image = btn_ResetZoom.Image;
                    zoomResetButton.ForeColor = btn_ResetZoom.ForeColor;
                    zoomResetButton.BackColor = btn_ResetZoom.BackColor;
                    zoomResetButton.Size = btn_ResetZoom.Size;
                    zoomResetButton.Location = new Point(_fsForm.Width - 215, _fsForm.Height - 520);
                    zoomResetButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    zoomResetButton.Click += btn_ResetZoom_Click;
                    _fsForm.Controls.Add(zoomResetButton);
                    zoomResetButton.BringToFront();

                    ButtonNoTheme zoomInButton = new ButtonNoTheme();
                    zoomInButton.Name = "zoomInButton";
                    zoomInButton.Text = btn_ZoomPlus.Text;
                    zoomInButton.Font = btn_ZoomPlus.Font;
                    zoomInButton.ForeColor = btn_ZoomPlus.ForeColor;
                    zoomInButton.BackColor = btn_ZoomPlus.BackColor;
                    zoomInButton.Size = btn_ZoomPlus.Size;
                    zoomInButton.Location = new Point(_fsForm.Width - 215, _fsForm.Height - 415);
                    zoomInButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    zoomInButton.MouseDown += btn_ZoomPlus_MouseDown;
                    zoomInButton.MouseUp += btn_Zoom_MouseUp;
                    _fsForm.Controls.Add(zoomInButton);
                    zoomInButton.BringToFront();

                    ButtonNoTheme zoomOutButton = new ButtonNoTheme();
                    zoomOutButton.Name = "zoomOutButton";
                    zoomOutButton.Text = btn_ZoomMinus.Text;
                    zoomOutButton.Font = btn_ZoomMinus.Font;
                    zoomOutButton.ForeColor = btn_ZoomMinus.ForeColor;
                    zoomOutButton.BackColor = btn_ZoomMinus.BackColor;
                    zoomOutButton.Size = btn_ZoomMinus.Size;
                    zoomOutButton.Location = new Point(_fsForm.Width - 215, _fsForm.Height - 310);
                    zoomOutButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
                    zoomOutButton.MouseDown += btn_ZoomMinus_MouseDown;
                    zoomOutButton.MouseUp += btn_Zoom_MouseUp;
                    _fsForm.Controls.Add(zoomOutButton);
                    zoomOutButton.BringToFront();

                    _fsForm.Show();
                }
                else
                {
                    //_fsForm.Close();
                    //_fsForm.Dispose();
                    //_fsForm = null;
                    _fsForm.Show();
                }
            }
            catch
            {
                if(MainV2.instance.devmode)
                    CustomMessageBox.Show("Teljes képernyős mód hiba");
            }

        }

        private void _fsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _fsForm.Controls.Remove(vv_VLC);
            this.tlp_CVBase.Controls.Add(vv_VLC);
            vv_VLC.Dock = DockStyle.Fill;
            vv_VLC.BringToFront();
            _mediaPlayer.Fullscreen = true;
            _fsForm.FormClosing -= _fsForm_FormClosing;
            _fsForm.Dispose();
            _fsForm = null;
        }


        private void btn_ResetZoom_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.ResetZoom();
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
                //CameraHandler.Instance.SetMode(CameraHandler.Instance.PrevCameraMode);

                CameraHandler.Instance.SetMode(NvSystemModes.Observation);

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
            if (_isFPVModeActive)
                CameraHandler.Instance.SetMode(NvSystemModes.Stow);
            else
                CameraHandler.Instance.StopTracking(true);

            IsCameraTrackingModeActive = false;
            SetStopButtonVisibility();
        }


        private void CameraHandler_event_ReportArrived(object sender, ReportEventArgs e)
        {
            try
            {
                if (isCameraConnected == false)
                {
                    try
                    {
                        if (bool.Parse(SettingManager.Get(Setting.AutoRecordVideoStream)) && _recordingInProgress == false)
                        {
                            StartRecording();

                            //set recording button
                            if (InvokeRequired)
                                Invoke(new Action(() =>
                                {
                                    this.btn_Recording.ForeColor = Color.Red;
                                }));
                            else
                            {
                                this.btn_Recording.ForeColor = Color.Red;
                            }

                            if (MainV2.instance.devmode)
                                CustomMessageBox.Show("A videó rögzítés elindult");
                        }
                    }
                    catch
                    {
                        
                    }
                    

                }

                //can not switch off trip until report in case of tripautoconnect
                if (InvokeRequired)
                    Invoke(new Action(() => { this.btn_TripSwitchOnOff.Enabled = true; }));
                else
                    this.btn_TripSwitchOnOff.Enabled = true;


                isCameraConnected = true;
                SetTripOnOffButton(true);
                try
                {
                    _cameraState = CameraHandler.Instance.SysReportModeToMavProtoMode(e.Report);
                    
                }
                catch { }

                string systemModeStr = CameraHandler.Instance.SysReportModeToMavProtoMode(e.Report).ToString();

                //Test: Set Camera Status
                if (InvokeRequired)
                    Invoke(new Action(() => { SetCameraStatusValue(systemModeStr); }));
                else
                    SetCameraStatusValue(systemModeStr);

                if (InvokeRequired)
                    Invoke(new Action(() => { SetGCSStatusValue(); }));
                else
                    SetGCSStatusValue();

                SetDroneStatusValue();

                if (_needToResetTime)
                {
                    CameraHandler.Instance.SetSystemTimeToCurrent();
                    _needToResetTime = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void StateHandler_MV04StateChange(object sender, MV04StateChangeEventArgs e)
        {
            if (InvokeRequired)
                Invoke(new Action(() => ReportStatusError()));
            else
                ReportStatusError();

            switch (e.NewState)
            {
                case MV04_State.Manual:
                    Execute_Manual_Tasks();
                    break;
                case MV04_State.TapToFly:
                    break;
                case MV04_State.Auto:
                    Execute_Auto_Tasks();
                    break;
                case MV04_State.Follow:
                    Execute_Follow_Tasks();
                    break;
                case MV04_State.Takeoff:
                    Execute_TakeOff_Tasks();
                    break;
                case MV04_State.RTL:
                    Execute_RTL_Tasks();
                    break;
                case MV04_State.Land:
                    Execute_Land_Tasks();
                    break;
                case MV04_State.Unknown:
                    break;
                default:
                    break;

            }
        }

        private void Execute_Auto_Tasks()
        {
            if (_feedTimer.Enabled)
                StopFeed();
        }

        private void Execute_Follow_Tasks()
        {
            if (_cameraState != NvSystemModes.Tracking)
            {
                CustomMessageBox.Show("A kamerának tracking üzemmódban kell lennie a követés indításához!");
            }
            else
            {
                StartFeed();
            }
        }

        private void Execute_RTL_Tasks()
        {
        }

        private void Execute_TakeOff_Tasks()
        {
            if (_feedTimer.Enabled)
                StopFeed();

            CameraHandler.Instance.SetMode(NvSystemModes.Stow);
        }

        private void Execute_Land_Tasks()
        {
            if (_feedTimer.Enabled)
                StopFeed();

            CameraHandler.Instance.SetMode(NvSystemModes.Nadir);
        }

        private void Execute_Manual_Tasks()
        {
            if (_feedTimer.Enabled)
                StopFeed();

        }

        private void _droneStatustimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CameraView.instance.IsDisposed)
                return;

            if (InvokeRequired)
                Invoke(new Action(() => { SetGCSStatusValue(); }));
            else
                SetGCSStatusValue();

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

        private void btn_TripSwitchOnOff_Click(object sender, EventArgs e)
        {
            SetTripOnOffButton(!MainV2.instance.TRIPRelayState);
            MainV2.instance.SwitchTRIPRelay(!MainV2.instance.TRIPRelayState);
        }

        private async void MainV2_RelaySwitched(object sender, MainV2.RelaySwitchEventArgs e)
        {
            if (e.Channel == MainV2.instance.TRIPRelayChannel)
            {
                if (e.State)
                {
                    try
                    {
                        await Task.Run(() => this.reconnectLoop());
                    }
                    catch
                    {

                    }
                }
            }
        }

        private int errorCounter = 0;
        private async Task reconnectLoop()
        {
            await Task.Run(async () => {
                while (!isCameraConnected)
                {
                    try
                    {
                        await Task.Delay(3000);
                        StartCameraControl();
                        //await reconnectControlDelayed();
                        _needToResetTime = true;
                    }
                    catch
                    {
                        errorCounter++;

                        if(errorCounter > 5)
                        {
                            break;
                        }
                        log.Error("reconnect loop exception");
                    }
                }

                Thread.Sleep(2000);
                CameraView.instance.StartVideoStreamVLC();
            });
        }

        private async Task reconnectControlDelayed()
        {
            Task.Delay(3000);
            StartCameraControl();

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


        private void SetCameraStatusValue(string st)
        {
            this.lb_CameraStatusValue.Text = st;
        }

        private void SetDroneStatusValue()
        {
            if (InvokeRequired)
                Invoke(new Action(() => { this.lb_DroneStatusValue.Text = MainV2.comPort.MAV.cs.mode; }));
            else
                this.lb_DroneStatusValue.Text = MainV2.comPort.MAV.cs.mode;
        }

        private void SetGCSStatusValue()
        {
            switch (StateHandler.CurrentSate)
            {
                case MV04_State.Manual:
                    lb_GCSSelectedStateValue.Text = "Loiter";
                    break;
                case MV04_State.TapToFly:
                    lb_GCSSelectedStateValue.Text = "Tap2Fly";
                    break;
                case MV04_State.Auto:
                    lb_GCSSelectedStateValue.Text = "Auto";
                    break;
                case MV04_State.Follow:
                    lb_GCSSelectedStateValue.Text = "Follow";
                    break;
                case MV04_State.RTL:
                    lb_GCSSelectedStateValue.Text = "RTL";
                    break;
                case MV04_State.Land:
                    lb_GCSSelectedStateValue.Text = "Land";
                    break;
                case MV04_State.Takeoff:
                    lb_GCSSelectedStateValue.Text = "TakeOff";
                    break;
                case MV04_State.FPV:
                    lb_GCSSelectedStateValue.Text = "FPV";
                    break;
                case MV04_State.Unknown:
                    lb_GCSSelectedStateValue.Text = "Unknown";
                    break;
                default:
                    break;
            }
        }

        private void ReportStatusError()
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
                //Task.Run(() => ProvideCameraError());
                lb_CameraStatusValue.Text = "Error";
                return;
            }

            NvSystemModes cameraMode = CameraHandler.Instance.SysReportModeToMavProtoMode((SysReport)CameraHandler.Instance.CameraReports[MavReportType.SystemReport]);

            switch (StateHandler.CurrentSate)
            {
                case MV04_State.Manual:
                    if(  MainV2.comPort.MAV.cs.mode.ToUpper() != "LOITER")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.TapToFly:
                    if (MainV2.comPort.MAV.cs.mode.ToUpper() != "GUIDED")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.Auto:
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
                    if (MainV2.comPort.MAV.cs.mode.ToUpper() != "GUIDED")
                    {
                        Task.Run(() => ProvideDroneError());
                    }
                    break;
                case MV04_State.FPV:
                    if (cameraMode != NvSystemModes.Stow)
                    {
                        Task.Run(() => ProvideCameraError());
                    }
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
            //BlinkControl(this.pnl_Camera);
        }

        private void ProvideDroneError()
        {
            //BlinkControl(this.pnl_Drone);
        }

        private void ProvideGCSError()
        {
            //BlinkControl(this.pnl_GCS);
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
            try
            {
                if (_libVlc == null)
                    _libVlc = new LibVLCSharp.Shared.LibVLC();

                if (_mediaPlayer == null)
                    _mediaPlayer = new MediaPlayer(_libVlc);

                // media has to be created new, because media.Mrl is read-only otherwise
                media = new Media(_libVlc, new Uri(SettingManager.Get(Setting.CameraStreamUrl)));

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
                }
                panelDoubleClick.MouseDoubleClick += new MouseEventHandler(vv_VLC_MouseDoubleClick);

                vv_VLC.ThisReallyVisible();
                vv_VLC.ChildReallyVisible();
                _mediaPlayer.Play(media);

                log.Info($"VLC started on {media.Mrl}");
            }
            catch (Exception ex)
            {
                // No messagebox, because reconnectLoop calls this method repeatedly
                log.Error($"Error at VLC start: {ex.Message}");
            }
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

            log.Info("VLC stopped");
        }

        private void SetTripOnOffButton(bool tripState)
        {
            Action setButton = () =>
            {
                if (tripState)
                {
                    btn_TripSwitchOnOff.Image = global::MissionPlanner.Properties.Resources.icons8_power_64_green;
                }
                else
                {
                    btn_TripSwitchOnOff.Image = global::MissionPlanner.Properties.Resources.icons8_power_64_inv;
                }
            };

            if (InvokeRequired)
            {
                Invoke(setButton);
            }
            else
            {
                setButton();
            }
        }

        private void btn_Surveillance_Click(object sender, EventArgs e)
        {
            NvSystemModes currentMode = CameraHandler.Instance.HasCameraReport(MavReportType.SystemReport) ?
                    CameraHandler.Instance.SysReportModeToMavProtoMode((SysReport)CameraHandler.Instance.CameraReports[MavReportType.SystemReport]) :
                    NvSystemModes.GRR;


            if (InvokeRequired)
                Invoke(new Action(() =>
                {
                    this.btn_FPVCameraMode.BackColor = Color.Black;
                }));
            else
            {
                this.btn_FPVCameraMode.BackColor = Color.Black;
            }

            if (currentMode == NvSystemModes.GRR)
            {
                //set to obs
                CameraHandler.Instance.SetMode(NvSystemModes.Observation);
                this.btn_Surveillance.Text = "GRR";
            }
            else
            {
                //set to grr
                CameraHandler.Instance.SetMode(NvSystemModes.GRR);
                this.btn_Surveillance.Text = "Observation";
            }

            _isFPVModeActive = false;
        }

        private void btn_NUC_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.DoNUC();
        }

        private void btn_Polarity_Click(object sender, EventArgs e)
        {
            //ToggleIRPolarity();

            if (isPolarityInverted)
            {
                CameraHandler.Instance.SetIRPolarity(IRPolarity.Normal);
                isPolarityInverted = false;
            }
            else
            {
                CameraHandler.Instance.SetIRPolarity(IRPolarity.Inverted);
                isPolarityInverted = true;
            }
        }

        private void btn_ZoomPlus_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetZoomLevel(CameraHandler.Instance.ZoomLevel + 1);
        }

        private void btn_ZoomMinus_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetZoomLevel(CameraHandler.Instance.ZoomLevel - 1);
        }

        private void btn_Zoom_MouseUp(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetZoom(ZoomState.Stop);
        }

        private void btn_Recording_Click(object sender, EventArgs e)
        {
            if(_recordingInProgress)
            {
                StopRecording();

                this.btn_Recording.Text = "Start Recording";
                this.btn_Recording.ForeColor = Color.Red;
            }
            else
            {
                StartRecording();

                this.btn_Recording.Text = "Stop Recording";
                this.btn_Recording.ForeColor = Color.White;
            }
        }
    }
}
