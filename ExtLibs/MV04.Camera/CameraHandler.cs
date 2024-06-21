using CoordinateSharp;
using MissionPlanner.Utilities;
using MV04.Settings;
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

    public enum NVColor
    {               // color,   polarity
        WhiteHot,   // 0,       0
        BlackHot,   // 0,       1
        Color,      // 1,       0
        ColorInverse// 1,       1
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

    #endregion

    public class ReportEventArgs : EventArgs
    {
        public MavProto.SysReport Report {get;set;}
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
                _VideoControl.VideoControlGetVersion(ref version.major, ref version.minor, ref version.build);
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

        #endregion

        #region Camera control Fields

        public IPAddress CameraControlIP { get; set; }
        public int CameraControlPort { get; set; }

        private MavProto.NvSystemModes PrevCameraMode;

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
                MavProto.GetMavManagerVersion(ref version.major, ref version.minor, ref version.build);
                return version;
            }
        }

        public bool IsCameraControlConnected
        {
            get
            {
                return MavProto.IsValid(_mavProto);
            }
        }

        /// <summary>
        /// Store Callback reports for state and drawing
        /// </summary>
        public Dictionary<MavProto.MavReportType, object> CameraReports { get; set; }

        /// <summary>
        /// Store last movement
        /// </summary>
        public (double yaw, double pitch, int zoom) LastMovement { get; private set; }

        public ZoomState LastZoomState { get; private set; }

        #endregion

        #endregion

        #region Constructor

        private CameraHandler()
        {
            GetIPAndPortFromSettings();

            _mavProto = new MavProto(2, CameraControlIP, CameraControlPort, OnReport, OnAck);
            _VideoControl = new VideoControl();

            StartCommunicationWithdevice();

            if (!MavProto.IsValid(_mavProto))
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

            return MavProto.IsValid(_mavProto);
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

        #region Methods

        public bool HasCameraReport(MavProto.MavReportType report_type)
        {
            if (CameraReports == null) CameraReports = new Dictionary<MavProto.MavReportType, object>();
            return CameraReports.ContainsKey(report_type) && CameraReports[report_type] != null;
        }

        private void OnReport(UInt32 report_type, IntPtr buf, UInt32 buf_len)
        {
            if (buf_len > 0)
            {
                byte[] packet = new byte[buf_len];
                Marshal.Copy(buf, packet, 0, packet.Length);

                if (!HasCameraReport((MavProto.MavReportType)report_type))
                    CameraReports.Add((MavProto.MavReportType)report_type, null);

                switch ((MavProto.MavReportType)report_type)
                {
                    case MavProto.MavReportType.SystemReport:
                        var sr = new MavProto.SysReport();
                        MavProto.MavlinkParseSysReport(packet, ref sr);
                        CameraReports[MavProto.MavReportType.SystemReport] = sr;

                        if (event_ReportArrived != null)
                            event_ReportArrived(null, new ReportEventArgs { Report = sr });

                        break;
                    case MavProto.MavReportType.LosReport:
                        var lr = new MavProto.LosReport();
                        MavProto.MavlinkParseLosReport(packet, ref lr);
                        CameraReports[MavProto.MavReportType.LosReport] = lr;
                        break;
                    case MavProto.MavReportType.GndCrsReport:
                        var gcr = new MavProto.GndCrsReport();
                        MavProto.MavlinkParseGndCrsReport(packet, ref gcr);
                        CameraReports[MavProto.MavReportType.GndCrsReport] = gcr;
                        break;
                    case MavProto.MavReportType.SnapshotReport:
                        var ssr = new MavProto.SnapshotReport();
                        MavProto.MavlinkParseSnapshotReport(packet, ref ssr);
                        CameraReports[MavProto.MavReportType.SnapshotReport] = ssr;
                        break;
                    case MavProto.MavReportType.SDCardReport:
                        var sdcr = new MavProto.SDCardReport();
                        MavProto.MavlinkParseSDCardReport(packet, ref sdcr);
                        CameraReports[MavProto.MavReportType.SDCardReport] = sdcr;
                        break;
                    case MavProto.MavReportType.VideoReport:
                        var vr = new MavProto.VideoReport();
                        MavProto.MavlinkParseVideoReport(packet, ref vr);
                        CameraReports[MavProto.MavReportType.VideoReport] = vr;
                        break;
                    case MavProto.MavReportType.LosRateReport:
                        var lrr = new MavProto.LosRateReport();
                        MavProto.MavlinkParseLosRateReport(packet, ref lrr);
                        CameraReports[MavProto.MavReportType.LosRateReport] = lrr;
                        break;
                    case MavProto.MavReportType.ObjectDetectionReport:
                        var odr = new MavProto.ObjectDetectionReport();
                        MavProto.MavlinkParseObjectDetectionReport(packet, ref odr);
                        CameraReports[MavProto.MavReportType.ObjectDetectionReport] = odr;
                        break;
                    case MavProto.MavReportType.IMUReport:
                        var ir = new MavProto.IMUReport();
                        MavProto.MavlinkParseIMUReport(packet, ref ir);
                        CameraReports[MavProto.MavReportType.IMUReport] = ir;
                        break;
                    case MavProto.MavReportType.FireDetectionReport:
                        var fdr = new MavProto.FireDetectionReport();
                        MavProto.MavlinkParseFireDetectionReport(packet, ref fdr);
                        CameraReports[MavProto.MavReportType.FireDetectionReport] = fdr;
                        break;
                    case MavProto.MavReportType.TrackingReport:
                        var tr = new MavProto.TrackingReport();
                        MavProto.MavlinkParseTrackingReport(packet, ref tr);
                        CameraReports[MavProto.MavReportType.TrackingReport] = tr;
                        break;
                    case MavProto.MavReportType.LPRReport:
                        var lprr = new MavProto.LPRReport();
                        MavProto.MavlinkParseLPRReport(packet, ref lprr);
                        CameraReports[MavProto.MavReportType.LPRReport] = lprr;
                        break;
                    case MavProto.MavReportType.ARMarkerReport:
                        var armr = new MavProto.ARMarkerReport();
                        MavProto.MavlinkParseARMarkerReport(packet, ref armr);
                        CameraReports[MavProto.MavReportType.ARMarkerReport] = armr;
                        break;
                    case MavProto.MavReportType.ParameterReport:
                        var pr = new MavProto.ParameterReport();
                        MavProto.MavlinkParseParameterReport(packet, ref pr);
                        CameraReports[MavProto.MavReportType.ParameterReport] = pr;
                        break;
                    case MavProto.MavReportType.CarCountReport:
                        var ccr = new MavProto.CarCountReport();
                        MavProto.MavlinkParseCarCountReport(packet, ref ccr);
                        CameraReports[MavProto.MavReportType.CarCountReport] = ccr;
                        break;
                    case MavProto.MavReportType.OGLRReport:
                        var oglr = new MavProto.OGLRReport();
                        MavProto.MavlinkParseOGLRReport(packet, ref oglr);
                        CameraReports[MavProto.MavReportType.OGLRReport] = oglr;
                        break;
                    case MavProto.MavReportType.VMDReport:
                        var vmdr = new MavProto.VMDReport();
                        MavProto.MavlinkParseVMDReport(packet, ref vmdr);
                        CameraReports[MavProto.MavReportType.VMDReport] = vmdr;
                        break;
                    case MavProto.MavReportType.PLR_Report:
                        var plrr = new MavProto.PLR_Report();
                        MavProto.MavlinkParsePLRReport(packet, ref plrr);
                        CameraReports[MavProto.MavReportType.PLR_Report] = plrr;
                        break;
                    case MavProto.MavReportType.RangeFinderReport:
                        var rfr = new MavProto.RangeFinderReport();
                        MavProto.MavlinkParseRangeFinderReport(packet, ref rfr);
                        CameraReports[MavProto.MavReportType.RangeFinderReport] = rfr;
                        break;
                    case MavProto.MavReportType.AuxCameraControlReport:
                        var accr = new MavProto.ObjectAuxCameraDetectionReport();
                        MavProto.MavlinkParseAuxCameraControlReport(packet, ref accr);
                        CameraReports[MavProto.MavReportType.AuxCameraControlReport] = accr;
                        break;
                    case MavProto.MavReportType.Reserved:
                    default: break;
                }
            }
        }

        private void OnAck(Int32 command_id, Int32 result, IntPtr unused_arg, IntPtr handle, IntPtr buf, UInt32 buf_len)
        {
            // command_id = DO_DIGICAM_CONTROL (fixed) (the wrapper function to all TRIP control messages)
            // result = MAV_RESULT code
            // buf = The whole MavLink packet

            return;

            if (buf_len > 0)
            {
                byte[] packet = new byte[buf_len];
                Marshal.Copy(buf, packet, 0, packet.Length);

                string hexMsg = BitConverter.ToString(packet);
                MavProto.MAV_RESULT mavResult = (MavProto.MAV_RESULT)result;
                MavProto.MavCommands mavCommand = (MavProto.MavCommands)command_id;
            }
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

        #region Camera control functions

        public bool SetNVColor(NVColor color)
        {
            return IsCameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetIR_Color(CameraControl.mav_comm, CameraControl.ackCb, color == NVColor.WhiteHot || color == NVColor.BlackHot ? 0 : 1) == MavProto.mav_error.ok
                &&
                (MavProto.mav_error)MavProto.MavCmdSetIRPolarity(CameraControl.mav_comm, CameraControl.ackCb, color == NVColor.WhiteHot || color == NVColor.Color ? 0 : 1) == MavProto.mav_error.ok;

            // The second call will always return MAV_RESULT_FAILED
        }

        public bool SetImageSensor(bool night)
        {
            return IsCameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetDisplayedSensor(CameraControl.mav_comm, CameraControl.ackCb, night ? 1 : 0) == MavProto.mav_error.ok;
        }

        public bool SetMode(MavProto.NvSystemModes mode)
        {
            return IsCameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetSystemMode(CameraControl.mav_comm, CameraControl.ackCb, (int)mode) == MavProto.mav_error.ok;
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

        private int Constrain(int number, int minValue, int maxValue)
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
                if (HasCameraReport(MavProto.MavReportType.SystemReport))
                    PrevCameraMode = (MavProto.NvSystemModes)((MavProto.SysReport)CameraReports[MavProto.MavReportType.SystemReport]).systemMode;
                else
                    PrevCameraMode = MavProto.NvSystemModes.GRR;

                // Handle default tracking pos (center)
                Point _trackPos = trackPos ?? new Point(640, 360);

                // Constrain tracking pos
                _trackPos.X = Constrain(_trackPos.X, 0, 1280);
                _trackPos.Y = Constrain(_trackPos.Y, 0, 720);

                // Start tracking
                return (MavProto.mav_error)MavProto.MavCmdSetTrackingMode(CameraControl.mav_comm, CameraControl.ackCb, _trackPos.X, _trackPos.Y, (int)TrackerMode.Track, 0) == MavProto.mav_error.ok;
            }

                return false;
        }

        public bool StopTracking(bool resetToPrevMode = false)
        {
            if (IsCameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetTrackingMode(CameraControl.mav_comm, CameraControl.ackCb, 0, 0, (int)TrackerMode.Disable, 0) == MavProto.mav_error.ok)
            {
                if (resetToPrevMode) SetMode(PrevCameraMode);
                return true;
            }
            
                return false;
            }
        
        public bool Retract()
        {
            return IsCameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdDoMountControl(CameraControl.mav_comm, CameraControl.ackCb, (int)MavProto.MavMountMode.Retract) == MavProto.mav_error.ok;
        }

        public bool SetZoom(ZoomState zoomState)
        {
            if (IsCameraControlConnected && zoomState != LastZoomState
                &&
                (MavProto.mav_error)MavProto.MavCmdSetCameraZoom(CameraControl.mav_comm, CameraControl.ackCb, (int)zoomState) == MavProto.mav_error.ok)
            {
                LastZoomState = zoomState;
                return true;
            }

            return false;
        }

        public bool MoveCamera(double yaw, double pitch, int zoom, double groundAlt = 0)
        {
            if (IsCameraControlConnected)
            {
                // Constrain inputs
                yaw = Constrain(yaw, -1, 1);
                pitch = Constrain(pitch, -1, 1);
                zoom = Constrain(zoom, 0, 2);

                // Only send if new
                if ((yaw != LastMovement.yaw || pitch != LastMovement.pitch || zoom != LastMovement.zoom)
                    &&
                    (MavProto.mav_error)MavProto.MavCmdSetGimbal(CameraControl.mav_comm, CameraControl.ackCb, (float)yaw, (float)pitch, zoom, (float)groundAlt) == MavProto.mav_error.ok)
                {
                LastMovement = (yaw, pitch, zoom);
                    return true;
            }
            }

                return false;
        }

        public bool ResetZoom()
        {
            if (IsCameraControlConnected)
            {
                MavProto.MavCmdSetFOV(CameraControl.mav_comm, CameraControl.ackCb, 65);
                return true;
            }
            
                return false;
            }

        public bool DoBIT()
        {
            return IsCameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdDoBIT_Test(CameraControl.mav_comm, CameraControl.ackCb) == MavProto.mav_error.ok;
            }

        public bool DoNUC()
        {
            return IsCameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdDoNUC(CameraControl.mav_comm, CameraControl.ackCb) == MavProto.mav_error.ok;
        }

        #endregion

        #endregion
    }
}
