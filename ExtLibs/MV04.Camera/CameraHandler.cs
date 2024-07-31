using CoordinateSharp;
using MissionPlanner.Utilities;
using MV04.Settings;
using MV04.State;
using NextVisionVideoControlLibrary;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MV04.Camera.MavProto;

namespace MV04.Camera
{
    #region Enums

    public enum IRColor
    {
        Grayscale,
        Color
    }

    public enum IRPolarity
    {
        Normal,
        Inverted
    }

    public enum TrackerMode
    {
        TrackOnPosition,
        Enable,
        Track,
        ReTrack,
        Disable
    }

    public enum ZoomState
    {
        Stop,
        In,
        Out
    }

    public enum PitchDirection
    {
        Stop,
        Up,
        Down
    }

    public enum YawDirection
    {
        Stop,
        Left,
        Right
    }

    #endregion

    public class ReportEventArgs : EventArgs
    {
        public MavProto.SysReport Report { get; set; }
    }

    public class CameraHandler
    {
        public event EventHandler<ReportEventArgs> event_ReportArrived;

        #region Fields

        private static CameraHandler _Instance;

        public static CameraHandler Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new CameraHandler();
                return _Instance;
            }
        }

        public static readonly int TripChannelNumber = 0;

        private VideoControl _VideoControl = null;

        public VideoControl CameraVideoControl
        {
            get
            {
                return _VideoControl;
            }
        }

        public static int sysID = 1;

        public Coordinate DronePos = new Coordinate();

        public Coordinate TargPos = new Coordinate();

        #region Video Fields

        public (int major, int minor, int build) StreamDLLVersion
        {
            get
            {
                (int major, int minor, int build) version = (0, 0, 0);
                //_VideoControl.VideoControlGetVersion(ref version.major, ref version.minor, ref version.build);
                return version;
            }
        }

        public IPAddress StreamIP { get; set; }

        public int StreamPort { get; set; }

        public VideoDecoder.RawFrameReadyCB StreamNewFrameCb { private get; set; }

        public VideoControl.VideoControlClickDelegate StreamClickCb { private get; set; }

        private string _MediaSavePath;

        public string MediaSavePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_MediaSavePath))
                {

                    //Settings.Setting
                    _MediaSavePath = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_media" + Path.DirectorySeparatorChar;
                }

                if (!Directory.Exists(_MediaSavePath))
                {
                    Directory.CreateDirectory(_MediaSavePath);
                }

                return _MediaSavePath;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value) && Directory.Exists(value))
                {
                    _MediaSavePath = value;
                }
            }
        }

        private Timer RecordingTimer;

        public static readonly string url = @"videotestsrc pattern=pinwheel ! video/x-raw, width=1920, height=1080, framerate=30/1 ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink";
        public static readonly string urlNight = @"videotestsrc ! video/x-raw, width=1920, height=1080, framerate=30/1 ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink";

        #endregion

        #region Camera control Fields

        public IPAddress CameraControlIP { get; set; }

        public int CameraControlPort { get; set; }

        private NvSystemModes PrevCameraMode;

        private MavProto _mavProto = null;

        private MavProto CameraControl
        {
            get
            {
                return _mavProto;
            }
        }

        public (int major, int minor, int build) CameraControlDLLVersion
        {
            get
            {
                (int major, int minor, int build) version = (0, 0, 0);
                GetMavManagerVersion(ref version.major, ref version.minor, ref version.build);
                return version;
            }
        }

        public bool IsCameraControlConnected
        {
            get
            {
                return IsValid(_mavProto);
            }
        }

        /// <summary>
        /// Store Callback reports for state and drawing
        /// </summary>
        public Dictionary<MavReportType, object> CameraReports { get; set; }

        /// <summary>
        /// Timer loop for gimbal movement commands
        /// </summary>
        private System.Windows.Forms.Timer GimbalTimer;

        /// <summary>
        /// Store the next gimbal movement
        /// </summary>
        public (double yaw, double pitch) NextMovement { get; private set; }

        /// <summary>
        /// Store the last gimbal movement
        /// </summary>
        public (double yaw, double pitch) LastMovement { get; private set; }

        /// <summary>
        /// Store the last zoom command
        /// </summary>
        public ZoomState LastZoomState { get; private set; }

        /// <summary>
        /// Stire the currently selected IR color mode
        /// </summary>
        public IRColor CurrentIRColor { get; private set; }

        #endregion

        #endregion

        #region Constructor

        private CameraHandler()
        {
            GetIPAndPortFromSettings();

            _mavProto = new MavProto(2, CameraControlIP, CameraControlPort, OnReport, OnAck);
            //_VideoControl = new VideoControl();

            StartCommunicationWithdevice();

            if (!IsValid(_mavProto))
            {
                MessageBox.Show("Invalid MavProto - CameraHandler - ctor");
            }
        }

        #endregion

        #region Init

        private void GetIPAndPortFromSettings()
        {
            CameraControlIP = IPAddress.Parse(SettingManager.Get(Setting.CameraControlIP));
            CameraControlPort = int.Parse(SettingManager.Get(Setting.CameraControlPort));

            if (CameraControlPort == 0)
                CameraControlPort = CameraControlPort == 0 ? 10024 : CameraControlPort;
            if (CameraControlIP == null)
                CameraControlIP = CameraControlIP ?? new IPAddress(new byte[] { 192, 168, 0, 203 });
        }

        /// <summary>
        /// Without this the camera does not trigger the OnReport callback
        /// </summary>
        private void StartCommunicationWithdevice()
        {
            byte[] packet = null; // Those are not the true bytes sent to the TRIP by the DLL but it attempts to be equal to it 
            /* Send Heart Beat mavlink message */
            _mavProto.MavlinkSendHeartbeatMsg(ref packet, false);

        }

        public bool CameraControlConnect(IPAddress ip, int port)
        {
            CameraControlIP = ip;
            CameraControlPort = port;

            GimbalTimer = new System.Windows.Forms.Timer();
            GimbalTimer.Enabled = false;
            GimbalTimer.Tick += GimbalTimer_Tick;

            return IsValid(_mavProto);
        }

        public bool StartStream(IPAddress ip, int port, VideoDecoder.RawFrameReadyCB onNewFrame, VideoControl.VideoControlClickDelegate onClick)
        {
            StreamIP = ip;
            StreamPort = port;
            StreamNewFrameCb = onNewFrame;
            StreamClickCb = onClick;

            return _VideoControl.VideoControlStartStream(StreamIP.ToString(), StreamPort, StreamNewFrameCb, StreamClickCb, false) == 0;
        }

        #endregion


        string currentStream = "";
        System.Threading.Thread currentGS;
        public void StartGstreamer(string u)
        {
            if (u == currentStream)
                return;
            currentStream = u;
            currentGS = GStreamer.StartA(u);
        }

        public void StopGstreamer()
        {
            if(currentGS != null)
            {
                currentGS.Abort();
                System.Threading.Thread.Sleep(100);
                currentGS = null;
            }
            
        }



        #region Methods

        public bool HasCameraReport(MavReportType report_type)
        {
            if (CameraReports == null) CameraReports = new Dictionary<MavReportType, object>();
            return CameraReports.ContainsKey(report_type) && CameraReports[report_type] != null;
        }

        private void OnReport(UInt32 report_type, IntPtr buf, UInt32 buf_len)
        {
            if (buf_len > 0)
            {
                byte[] packet = new byte[buf_len];
                Marshal.Copy(buf, packet, 0, packet.Length);

                string hexMsg = BitConverter.ToString(packet);

                if (!HasCameraReport((MavReportType)report_type))
                    CameraReports.Add((MavReportType)report_type, null);

                switch ((MavReportType)report_type)
                {
                    case MavProto.MavReportType.SystemReport:
                        var sr = new MavProto.SysReport();
                        MavProto.MavlinkParseSysReport(packet, ref sr);
                        CameraReports[MavProto.MavReportType.SystemReport] = sr;

                        if (event_ReportArrived != null)
                            event_ReportArrived(null, new ReportEventArgs { Report = sr });

                        break;
                    case MavReportType.LosReport:
                        var lr = new LosReport();
                        MavlinkParseLosReport(packet, ref lr);
                        CameraReports[MavReportType.LosReport] = lr;
                        break;
                    case MavReportType.GndCrsReport:
                        var gcr = new GndCrsReport();
                        MavlinkParseGndCrsReport(packet, ref gcr);
                        CameraReports[MavReportType.GndCrsReport] = gcr;
                        break;
                    case MavReportType.SnapshotReport:
                        var ssr = new SnapshotReport();
                        MavlinkParseSnapshotReport(packet, ref ssr);
                        CameraReports[MavReportType.SnapshotReport] = ssr;
                        break;
                    case MavReportType.SDCardReport:
                        var sdcr = new SDCardReport();
                        MavlinkParseSDCardReport(packet, ref sdcr);
                        CameraReports[MavReportType.SDCardReport] = sdcr;
                        break;
                    case MavReportType.VideoReport:
                        var vr = new VideoReport();
                        MavlinkParseVideoReport(packet, ref vr);
                        CameraReports[MavReportType.VideoReport] = vr;
                        break;
                    case MavReportType.LosRateReport:
                        var lrr = new LosRateReport();
                        MavlinkParseLosRateReport(packet, ref lrr);
                        CameraReports[MavReportType.LosRateReport] = lrr;
                        break;
                    case MavReportType.ObjectDetectionReport:
                        var odr = new ObjectDetectionReport();
                        MavlinkParseObjectDetectionReport(packet, ref odr);
                        CameraReports[MavReportType.ObjectDetectionReport] = odr;
                        break;
                    case MavReportType.IMUReport:
                        var ir = new IMUReport();
                        MavlinkParseIMUReport(packet, ref ir);
                        CameraReports[MavReportType.IMUReport] = ir;
                        break;
                    case MavReportType.FireDetectionReport:
                        var fdr = new FireDetectionReport();
                        MavlinkParseFireDetectionReport(packet, ref fdr);
                        CameraReports[MavReportType.FireDetectionReport] = fdr;
                        break;
                    case MavReportType.TrackingReport:
                        var tr = new TrackingReport();
                        MavlinkParseTrackingReport(packet, ref tr);
                        CameraReports[MavReportType.TrackingReport] = tr;
                        break;
                    case MavReportType.LPRReport:
                        var lprr = new LPRReport();
                        MavlinkParseLPRReport(packet, ref lprr);
                        CameraReports[MavReportType.LPRReport] = lprr;
                        break;
                    case MavReportType.ARMarkerReport:
                        var armr = new ARMarkerReport();
                        MavlinkParseARMarkerReport(packet, ref armr);
                        CameraReports[MavReportType.ARMarkerReport] = armr;
                        break;
                    case MavReportType.ParameterReport:
                        var pr = new ParameterReport();
                        MavlinkParseParameterReport(packet, ref pr);
                        CameraReports[MavReportType.ParameterReport] = pr;
                        break;
                    case MavReportType.CarCountReport:
                        var ccr = new CarCountReport();
                        MavlinkParseCarCountReport(packet, ref ccr);
                        CameraReports[MavReportType.CarCountReport] = ccr;
                        break;
                    case MavReportType.OGLRReport:
                        var oglr = new OGLRReport();
                        MavlinkParseOGLRReport(packet, ref oglr);
                        CameraReports[MavReportType.OGLRReport] = oglr;
                        break;
                    case MavReportType.VMDReport:
                        var vmdr = new VMDReport();
                        MavlinkParseVMDReport(packet, ref vmdr);
                        CameraReports[MavReportType.VMDReport] = vmdr;
                        break;
                    case MavReportType.PLR_Report:
                        var plrr = new PLR_Report();
                        MavlinkParsePLRReport(packet, ref plrr);
                        CameraReports[MavReportType.PLR_Report] = plrr;
                        break;
                    case MavReportType.RangeFinderReport:
                        var rfr = new RangeFinderReport();
                        MavlinkParseRangeFinderReport(packet, ref rfr);
                        CameraReports[MavReportType.RangeFinderReport] = rfr;
                        break;
                    case MavReportType.AuxCameraControlReport:
                        var accr = new ObjectAuxCameraDetectionReport();
                        MavlinkParseAuxCameraControlReport(packet, ref accr);
                        CameraReports[MavReportType.AuxCameraControlReport] = accr;
                        break;
                    case MavReportType.Reserved:
                    default: break;
                }
            }
        }

        private void OnAck(Int32 command_id, Int32 result, IntPtr unused_arg, IntPtr handle, IntPtr buf, UInt32 buf_len)
        {
            // command_id = DO_DIGICAM_CONTROL (fixed) (the wrapper function to all TRIP control messages)
            // result = MAV_RESULT code
            // buf = The whole MavLink packet

            if (buf_len > 0)
            {
                byte[] packet = new byte[buf_len];
                Marshal.Copy(buf, packet, 0, packet.Length);

                string hexMsg = BitConverter.ToString(packet);
                MAV_RESULT mavResult = (MAV_RESULT)result;
            }

            return;
        }

        #region Video Methods

        public bool DoPhoto()
        {
            if (_VideoControl != null)
            {
                string sepChar = "_";
                string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string droneID = sysID.ToString().PadLeft(3, '0');
                string dronePos = DronePos.UTM.ToString().Replace(" ", "");
                string targPos = TargPos.UTM.ToString().Replace(" ", "");

                string filePath = MediaSavePath + dateTime + sepChar + droneID + sepChar + dronePos + sepChar + targPos;

                OpenGLControl oglc = _VideoControl.Controls[0] as OpenGLControl;
                OpenGL ogl = oglc.OpenGL;

                Bitmap bmp = new Bitmap(oglc.Width, oglc.Height);
                BitmapData bmpd = bmp.LockBits(oglc.Bounds, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                ogl.ReadPixels(0, 0, oglc.Width, oglc.Height, OpenGL.GL_BGR, OpenGL.GL_UNSIGNED_BYTE, bmpd.Scan0);
                bmp.UnlockBits(bmpd);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

                bmp.Save(filePath + ".png", ImageFormat.Png);

                return File.Exists(filePath + ".png");
            }
            else
            {
                return false;
            }
        }
        public bool StartRecording(TimeSpan? segmentLength)
        {
            if (_VideoControl != null)
            {
                if (segmentLength.HasValue)
                {
                    RecordingTimer = new Timer()
                    {
                        Enabled = false,
                        Interval = (int)Math.Round(segmentLength.Value.TotalMilliseconds)
                    };

                    RecordingTimer.Tick += (sender, eventArgs) =>
                    {
                        // Stop & save previous recording
                        _VideoControl.VideoControlStopRec();

                        string _sepChar = "_";
                        string _dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string _droneID = sysID.ToString().PadLeft(3, '0');
                        string _dronePos = DronePos.UTM.ToString().Replace(" ", "");
                        string _targPos = TargPos.UTM.ToString().Replace(" ", "");
                        string _filePath = MediaSavePath + _dateTime + _sepChar + _droneID + _sepChar + _dronePos + _sepChar + _targPos;

                        // Start new recording
                        _VideoControl.VideoControlStartRec(_filePath + ".ts");
                    };

                    // Start recording timer
                    RecordingTimer.Start();
                }

                string sepChar = "_";
                string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string droneID = sysID.ToString().PadLeft(3, '0');
                string dronePos = DronePos.UTM.ToString().Replace(" ", "");
                string targPos = TargPos.UTM.ToString().Replace(" ", "");
                string filePath = MediaSavePath + dateTime + sepChar + droneID + sepChar + dronePos + sepChar + targPos;

                // Start first recording
                _VideoControl.VideoControlStartRec(filePath + ".ts");

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool StopRecording()
        {
            if (_VideoControl != null)
            {
                _VideoControl.VideoControlStopRec();

                if (RecordingTimer != null)
                {
                    RecordingTimer.Stop();
                    RecordingTimer.Dispose();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Camera control Methods

        public bool SetIRColor(IRColor color)
        {
            if (IsCameraControlConnected
                &&
                (mav_error)MavCmdSetIR_Color(CameraControl.mav_comm, CameraControl.ackCb, (int)color) == mav_error.ok)
            {
                CurrentIRColor = color;
                return true;
            }
            return false;
        }

        public bool SetIRPolarity(IRPolarity polarity)
        {
            return IsCameraControlConnected
                &&
                (mav_error)MavCmdSetIRPolarity(CameraControl.mav_comm, CameraControl.ackCb, (int)polarity) == mav_error.ok;
        }

        public bool SetImageSensor(bool night)
        {
            return IsCameraControlConnected
                &&
                (mav_error)MavCmdSetDisplayedSensor(CameraControl.mav_comm, CameraControl.ackCb, night ? 1 : 0) == mav_error.ok;
        }

        public bool SetMode(NvSystemModes mode)
        {
            if (IsCameraControlConnected
                &&
                (mav_error)MavCmdSetSystemMode(CameraControl.mav_comm, CameraControl.ackCb, (int)mode) == mav_error.ok)
            {
                switch (mode)
                {
                    // Start gimbal moving timer in modes that support it
                    case NvSystemModes.HoldCoordinate:
                    case NvSystemModes.Observation:
                    case NvSystemModes.LocalPosition:
                    case NvSystemModes.GlobalPosition:
                    case NvSystemModes.GRR:
                    case NvSystemModes.PTC:
                    case NvSystemModes.UnstabilizedPosition:
                        StartGimbal();
                        break;
                    // Stop gimbal moving timer in modes that don't support it
                    default:
                        StopGimbal();
                        break;
                }
                return true;
            }
            return false;
        }

        public static Point FullSizeToTrackingSize(Point fullSizePoint, Size fullSizeResolution)
        {
            return new Point(
                (int)Math.Round(fullSizePoint.X * (1280.0 / fullSizeResolution.Width)),
                (int)Math.Round(fullSizePoint.Y * (720.0 / fullSizeResolution.Height)));
        }

        public static Point FullSizeToTrackingSize(Point fullSizePoint)
        {
            return FullSizeToTrackingSize(fullSizePoint, new Size(1920, 1080));
        }

        private double Constrain(double number, double minValue, double maxValue)
        {
            if (number < minValue)
            {
                return minValue;
            }
            else if (number > maxValue)
            {
                return maxValue;
            }
            else
            {
                return number;
            }
        }

        public int Constrain(int number, int minValue, int maxValue)
        {
            if (number < minValue)
            {
                return minValue;
            }
            else if (number > maxValue)
            {
                return maxValue;
            }
            else
            {
                return number;
            }
        }

        public bool StartTracking(Point? trackPos)
        {
            if (IsCameraControlConnected)
            {
                // Save current camera mode
                if (HasCameraReport(MavReportType.SystemReport))
                    PrevCameraMode = (NvSystemModes)((SysReport)CameraReports[MavReportType.SystemReport]).systemMode;
                else
                    PrevCameraMode = NvSystemModes.GRR;

                // Handle default tracking pos (center)
                Point _trackPos = trackPos ?? new Point(640, 360);

                // Constrain tracking pos
                _trackPos.X = Constrain(_trackPos.X, 0, 1280);
                _trackPos.Y = Constrain(_trackPos.Y, 0, 720);

                // Start tracking
                return (mav_error)MavCmdSetTrackingMode(CameraControl.mav_comm, CameraControl.ackCb, _trackPos.X, _trackPos.Y, (int)TrackerMode.Track, 0) == mav_error.ok;
            }

            return false;
        }

        public bool StopTracking(bool resetToPrevMode = false)
        {
            if (IsCameraControlConnected
                &&
                (mav_error)MavCmdSetTrackingMode(CameraControl.mav_comm, CameraControl.ackCb, 0, 0, (int)TrackerMode.Disable, 0) == mav_error.ok)
            {
                if (resetToPrevMode) SetMode(PrevCameraMode);
                return true;
            }

            return false;
        }

        public bool Retract()
        {
            return IsCameraControlConnected
                && (mav_error)MavCmdDoMountControl(CameraControl.mav_comm, CameraControl.ackCb, (int)MavMountMode.Retract) == mav_error.ok;
        }

        public bool SetZoom(ZoomState zoomState)
        {
            if (IsCameraControlConnected && zoomState != LastZoomState
                &&
                (mav_error)MavCmdSetCameraZoom(CameraControl.mav_comm, CameraControl.ackCb, (int)zoomState) == mav_error.ok)
            {
                LastZoomState = zoomState;
                return true;
            }

            return false;
        }

        public bool ResetZoom()
        {
            if (IsCameraControlConnected)
            {
                MavCmdSetFOV(CameraControl.mav_comm, CameraControl.ackCb, 65);
                return true;
            }

            return false;
        }

        private bool MoveCamera(double yaw, double pitch, double groundAlt = 0)
        {
            if (IsCameraControlConnected)
            {
                // Constrain inputs
                yaw = Constrain(yaw, -1, 1);
                pitch = Constrain(pitch, -1, 1);

                // Only send if new
                if ((yaw != LastMovement.yaw || pitch != LastMovement.pitch)
                    &&
                    (mav_error)MavCmdSetGimbal(CameraControl.mav_comm, CameraControl.ackCb, (float)yaw, (float)pitch, (int)LastZoomState, (float)groundAlt) == mav_error.ok)
                {
                    LastMovement = (yaw, pitch);
                    return true;
                }
            }

            return false;
        }

        public void StartGimbal(TimeSpan? timerInterval = null)
        {
            if (GimbalTimer.Enabled) GimbalTimer.Stop();

            if (timerInterval == null) timerInterval = TimeSpan.FromMilliseconds(100); // Default gimbal update frequency is 10Hz
            GimbalTimer.Interval = (int)Math.Round(timerInterval.Value.TotalMilliseconds);
            GimbalTimer.Start();
        }

        private void GimbalTimer_Tick(object sender, EventArgs e)
        {
            MoveCamera(NextMovement.yaw, NextMovement.pitch);
        }

        public void StopGimbal()
        {
            GimbalTimer.Stop();
        }

        public void SetCameraPitch(PitchDirection direction, double speed = 1)
        {
            double pitchValue;
            switch (direction)
            {
                case PitchDirection.Up:
                    pitchValue = Constrain(speed, 0, 1);        // Positive is up
                    break;
                case PitchDirection.Down:
                    pitchValue = Constrain(speed, 0, 1) * -1;   // Negative is down
                    break;
                default:
                    pitchValue = 0;
                    break;
            }
            NextMovement = (NextMovement.yaw, pitchValue);
        }

        public void SetCameraYaw(YawDirection direction, double speed = 1)
        {
            double yawValue;
            switch (direction)
            {
                case YawDirection.Left:
                    yawValue = Constrain(speed, 0, 1);      // Positive is left
                    break;
                case YawDirection.Right:
                    yawValue = Constrain(speed, 0, 1) * -1; // Negative is right
                    break;
                default:
                    yawValue = 0;
                    break;
            }
            NextMovement = (yawValue, NextMovement.pitch);
        }

        public bool DoBIT()
        {
            return IsCameraControlConnected
                && (mav_error)MavCmdDoBIT_Test(CameraControl.mav_comm, CameraControl.ackCb) == mav_error.ok;
        }

        public bool DoNUC()
        {
            return IsCameraControlConnected
                && (mav_error)MavCmdDoNUC(CameraControl.mav_comm, CameraControl.ackCb) == mav_error.ok;
        }

        #endregion

        #endregion
    }
}