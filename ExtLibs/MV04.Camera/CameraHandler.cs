using CoordinateSharp;
using MissionPlanner.Utilities;
using NextVisionVideoControlLibrary;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Camera
{
    public static class CameraHandler
    {
        #region VIDEO CONTROL
        // ============================== VIDEO CONTROL ==============================

        private static VideoControl _VideoControl = null;
        public static VideoControl VideoControl // Singleton prop
        {
            get
            {
                if (_VideoControl == null)
                {
                    _VideoControl = new VideoControl();
                }
                return _VideoControl;
            }
        }

        public static (int major, int minor, int build) StreamDLLVersion
        {
            get
            {
                (int major, int minor, int build) version = (0, 0, 0);
                VideoControl.VideoControlGetVersion(ref version.major, ref version.minor, ref version.build);
                return version;
            }
        }

        public static IPAddress StreamIP { get; set; }
        public static int StreamPort { get; set; }
        public static VideoDecoder.RawFrameReadyCB StreamNewFrameCb { private get; set; }
        public static VideoControl.VideoControlClickDelegate StreamClickCb { private get; set; }
        public static bool StartStream(IPAddress ip, int port, VideoDecoder.RawFrameReadyCB onNewFrame, VideoControl.VideoControlClickDelegate onClick)
        {
            StreamIP = ip;
            StreamPort = port;
            StreamNewFrameCb = onNewFrame;
            StreamClickCb = onClick;

            return VideoControl.VideoControlStartStream(StreamIP.ToString(), StreamPort, StreamNewFrameCb, StreamClickCb, false) == 0;
        }
        public static bool StartStream()
        {
            StreamIP = StreamIP ?? new IPAddress(new byte[] { 225, 1, 2, 3 });
            StreamPort = StreamPort == 0 ? 11024 : StreamPort;
            if (StreamNewFrameCb == null) StreamNewFrameCb = (frame_buf, status, width, height) => { };
            if (StreamClickCb == null) StreamClickCb = (x_pos, y_pos) => { };

            return VideoControl.VideoControlStartStream(StreamIP.ToString(), StreamPort, StreamNewFrameCb, StreamClickCb, false) == 0;
        }

        public static bool StopStream()
        {
            // TODO: Implement VideoControlStopStream()
            return true;
        }

        private static string _MediaSavePath;
        public static string MediaSavePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_MediaSavePath))
                {
                    _MediaSavePath = Settings.GetUserDataDirectory() + "MV04_media" + Path.DirectorySeparatorChar;
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

        public static int sysID = 1;
        public static Coordinate DronePos = new Coordinate();
        public static Coordinate TargPos = new Coordinate();

        public static bool DoPhoto()
        {
            if (_VideoControl != null)
            {
                string sepChar = "_";
                string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string droneID = sysID.ToString().PadLeft(3, '0');
                string dronePos = DronePos.UTM.ToString().Replace(" ", "");
                string targPos = TargPos.UTM.ToString().Replace(" ", "");

                string filePath = MediaSavePath + dateTime + sepChar + droneID + sepChar + dronePos + sepChar + targPos;

                OpenGLControl oglc = VideoControl.Controls[0] as OpenGLControl;
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

        private static Timer RecordingTimer;

        public static bool StartRecording(TimeSpan? segmentLength)
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
                        VideoControl.VideoControlStopRec();

                        string _sepChar = "_";
                        string _dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string _droneID = sysID.ToString().PadLeft(3, '0');
                        string _dronePos = DronePos.UTM.ToString().Replace(" ", "");
                        string _targPos = TargPos.UTM.ToString().Replace(" ", "");
                        string _filePath = MediaSavePath + _dateTime + _sepChar + _droneID + _sepChar + _dronePos + _sepChar + _targPos;

                        // Start new recording
                        VideoControl.VideoControlStartRec(_filePath + ".ts");
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
                VideoControl.VideoControlStartRec(filePath + ".ts");

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool StopRecording()
        {
            if (_VideoControl != null)
            {
                VideoControl.VideoControlStopRec();

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

        // ============================== VIDEO CONTROL ==============================
        #endregion

        #region CAMERA CONTROL
        // ============================== CAMERA CONTROL ==============================

        public static IPAddress CameraControlIP { get; set; }
        public static int CameraControlPort { get; set; }
        
        public static MavProto.MavPacketReceivedCallBack CameraControlReportReceivedCb {  private get; set; }
        public static MavProto.MavCbCmd CameraControlAckReceivedCb {  private get; set; }

        private static MavProto.NvSystemModes PrevCameraMode;

        private static MavProto _CameraControl = null;
        
        private static MavProto CameraControl // Singleton prop
        {
            get
            {
                if (_CameraControl == null)
                {
                    _CameraControl = new MavProto(2, CameraControlIP, CameraControlPort, CameraControlReportReceivedCb, CameraControlAckReceivedCb);
                }
                return _CameraControl;
            }
        }

        public static (int major, int minor, int build) CameraControlDLLVersion
        {
            get
            {
                (int major, int minor, int build) version = (0, 0, 0);
                MavProto.GetMavManagerVersion(ref version.major, ref version.minor, ref version.build);
                return version;
            }
        }

        public static bool CameraControlConnect(IPAddress ip, int port)
        {
            CameraControlIP = ip;
            CameraControlPort = port;
            CameraControlReportReceivedCb = OnReport;
            CameraControlAckReceivedCb = OnAck;
            
            return MavProto.IsValid(CameraControl); // This call creates singleton MavProto instance
        }
        public static bool CameraControlConnect()
        {
            CameraControlIP = CameraControlIP ?? new IPAddress(new byte[] { 192, 168, 0, 203 });
            CameraControlPort = CameraControlPort == 0 ? 10024 : CameraControlPort;
            CameraControlReportReceivedCb = OnReport;
            CameraControlAckReceivedCb = OnAck;

            return MavProto.IsValid(CameraControl); // This call creates singleton MavProto instance
        }

        public static bool CameraControlDisconnect()
        {
            CameraControl.MavProtoClose();  // Close sockets
            _CameraControl = null;          // Dispose MavProto instance
            return !CameraControlConnected;
        }

        public static bool CameraControlConnected
        {
            get
            {
                return MavProto.IsValid(_CameraControl);
            }
        }

        public static Dictionary<MavProto.MavReportType, object> CameraReports { get; set; }

        public static bool HasCameraReport(MavProto.MavReportType report_type) 
        {
            if (CameraReports == null) CameraReports = new Dictionary<MavProto.MavReportType, object>();
            return CameraReports.ContainsKey(report_type) && CameraReports[report_type] != null;
        }

        private static void OnReport(UInt32 report_type, IntPtr buf, UInt32 buf_len)
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

        private static void OnAck(Int32 command_id, Int32 result, IntPtr unused_arg, IntPtr handle, IntPtr buf, UInt32 buf_len)
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

        public enum NVColor
        {               // color,   polarity
            WhiteHot,   // 0,       0
            BlackHot,   // 0,       1
            Color,      // 1,       0
            ColorInverse// 1,       1
        }
        
        public static bool SetNVColor(NVColor color)
        {
            return CameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetIR_Color(CameraControl.mav_comm, CameraControl.ackCb, color == NVColor.WhiteHot || color == NVColor.BlackHot ? 0 : 1) == MavProto.mav_error.ok
                &&
                (MavProto.mav_error)MavProto.MavCmdSetIRPolarity(CameraControl.mav_comm, CameraControl.ackCb, color == NVColor.WhiteHot || color == NVColor.Color ? 0 : 1) == MavProto.mav_error.ok;

            // The second call will always return MAV_RESULT_FAILED
        }
        
        public static async Task<bool> SetNVColorAsync(NVColor color)
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived1 = new TaskCompletionSource<MavProto.MAV_RESULT>(),
                                                          CmdAckReceived2 = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent1 = (MavProto.mav_error)MavProto.MavCmdSetIR_Color(CameraControl.mav_comm,
                    (command_id, result, unused_arg, handle, buf, buf_len) =>
                    {
                        CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                        CmdAckReceived1.SetResult((MavProto.MAV_RESULT)result);
                    },
                    color == NVColor.WhiteHot || color == NVColor.BlackHot ? 0 : 1);

                await CmdAckReceived1.Task; // wait for the previous call to finish

                MavProto.mav_error CmdSent2 = (MavProto.mav_error)MavProto.MavCmdSetIRPolarity(CameraControl.mav_comm,
                    (command_id, result, unused_arg, handle, buf, buf_len) =>
                    {
                        CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                        CmdAckReceived2.SetResult((MavProto.MAV_RESULT)result);
                    },
                    color == NVColor.WhiteHot || color == NVColor.Color ? 0 : 1);

                return CmdSent1 == MavProto.mav_error.ok
                    && CmdAckReceived1.Task.Result == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED
                    && CmdSent2 == MavProto.mav_error.ok
                    && await CmdAckReceived2.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
                return false;
        }

        public static bool SetImageSensor(bool night)
        {
            return CameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetDisplayedSensor(CameraControl.mav_comm, CameraControl.ackCb, night ? 1 : 0) == MavProto.mav_error.ok;
        }
        
        public static async Task<bool> SetImageSensorAsync(bool night)
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetDisplayedSensor(CameraControl.mav_comm,
                    (command_id, result, unused_arg, handle, buf, buf_len) =>
                    {
                        CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                        CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                    },
                    night ? 1 : 0);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
                return false;
        }

        public static bool SetMode(MavProto.NvSystemModes mode)
        {
            return CameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetSystemMode(CameraControl.mav_comm, CameraControl.ackCb, (int)mode) == MavProto.mav_error.ok;
        }
        
        public static async Task<bool> SetModeAsync(MavProto.NvSystemModes mode)
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetSystemMode(CameraControl.mav_comm,
                    (command_id, result, unused_arg, handle, buf, buf_len) =>
                    {
                        CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                        CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                    },
                    (int)mode);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
                return false;
        }

        public enum TrackerMode
        {
            TrackOnPosition,
            Enable,
            Track,
            ReTrack,
            Disable
        }

        public static Point FullSizeToTrackingSize(Point fullSizePoint)
        {
            return new Point(
                (int)Math.Round(fullSizePoint.X * (1280.0 / 1920.0)),
                (int)Math.Round(fullSizePoint.Y * (720.0 / 1080.0)));
        }

        private static double Constrain(double number, double minValue, double maxValue)
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

        private static int Constrain(int number, int minValue, int maxValue)
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

        public static bool StartTracking(Point? trackPos)
        {
            if (CameraControlConnected)
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
        
        public static async Task<bool> StartTrackingAsync(Point? trackPos)
        {
            if (CameraControlConnected)
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

                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetTrackingMode(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                },
                _trackPos.X, _trackPos.Y,
                (int)TrackerMode.Track,
                0);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
                return false;
        }

        public static bool StopTracking(bool resetToPrevMode = false)
        {
            if (CameraControlConnected
                &&
                (MavProto.mav_error)MavProto.MavCmdSetTrackingMode(CameraControl.mav_comm, CameraControl.ackCb, 0, 0, (int)TrackerMode.Disable, 0) == MavProto.mav_error.ok)
            {
                if (resetToPrevMode) SetMode(PrevCameraMode);
                return true;
            }
            
                return false;
            }
        
        public static async Task<bool> StopTrackingAsync(bool resetToPrevMode = false)
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived1 = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetTrackingMode(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived1.SetResult((MavProto.MAV_RESULT)result);
                },
                0, 0,
                (int)TrackerMode.Disable,
                0);

                if (resetToPrevMode)    // Tracker & SetMode commands
                {
                    return CmdSent == MavProto.mav_error.ok
                        && await CmdAckReceived1.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED
                        && await SetModeAsync(PrevCameraMode);
                }
                else                    // Tracker command only
                {
                    return CmdSent == MavProto.mav_error.ok && await CmdAckReceived1.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
                }
            }
            
                return false;
        }

        public static bool Retract()
        {
            return CameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdDoMountControl(CameraControl.mav_comm, CameraControl.ackCb, (int)MavProto.MavMountMode.Retract) == MavProto.mav_error.ok;
        }

        public static async Task<bool> RetractAsync()
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdDoMountControl(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                }, (int)MavProto.MavMountMode.Retract);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }

            return false;
        }

        public enum ZoomState
        {
            Stop,
            In,
            Out
        }

        public static ZoomState LastZoomState { get; private set; }

        public static bool SetZoom(ZoomState zoomState)
        {
            if (CameraControlConnected && zoomState != LastZoomState
                &&
                (MavProto.mav_error)MavProto.MavCmdSetCameraZoom(CameraControl.mav_comm, CameraControl.ackCb, (int)zoomState) == MavProto.mav_error.ok)
            {
                LastZoomState = zoomState;
                return true;
            }

            return false;
        }

        public static async Task<bool> SetZoomAsync(ZoomState zoomState)
        {
            // Only send if new
            if (CameraControlConnected && zoomState != LastZoomState)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetCameraZoom(CameraControl.mav_comm, (command_id, result,unused_arg,   handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                }, (int)zoomState);

                LastZoomState = zoomState;

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }

                return false;
            }

        public static (double yaw, double pitch, int zoom) LastMovement { get; private set; }

        public static bool MoveCamera(double yaw, double pitch, int zoom, double groundAlt = 0)
        {
            if (CameraControlConnected)
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

        public static async Task<bool> MoveCameraAsync(double yaw, double pitch, int zoom, double groundAlt = 0)
        {
            // DO_DIGICAM_COMMAND doesn't return a COMMAND_ACK
            return MoveCamera(yaw, pitch, zoom, groundAlt);
        }

        public static bool ResetZoom()
        {
            if (CameraControlConnected)
            {
                MavProto.MavCmdSetFOV(CameraControl.mav_comm, CameraControl.ackCb, 65);
                return true;
            }
            
                return false;
            }
        
        public static async Task<bool> ResetZoomAsync()
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetFOV(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                },
                65);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
                return false;
        }

        public static bool DoBIT()
        {
            return CameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdDoBIT_Test(CameraControl.mav_comm, CameraControl.ackCb) == MavProto.mav_error.ok;
            }
            
        public static async Task<bool> DoBITAsync()
        {
            if (CameraControlConnected)
            {
                // Add signaler to Report Receiver task
                // Check wether the system mode transition has happened
                // Wait until mode is BIT, then wait until mode is not BIT
                // Return afterwards

                MavProto.MavCmdDoBIT_Test(CameraControl.mav_comm, CameraControl.ackCb);

                // Await reported system mode to cycle between BIT and anything else

                return true;
            }
            
                return false;
        }

        public static bool DoNUC()
        {
            return CameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdDoNUC(CameraControl.mav_comm, CameraControl.ackCb) == MavProto.mav_error.ok;
            }
        
        public static async Task<bool> DoNUCAsync()
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdDoNUC(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                });

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
                return false;
        }

        public static async Task<bool> DoFullTestAsync()
        {
            // TODO: Implement DoFullTestAsync()

            // Set to Visible -> BIT -> Set to NV -> NUC -> Set to whatever it was before the call

            return false;
        }

        public enum OSDMode
        {
            Disabled,
            Operational1,
            Operational2,
            Development,
            DevelopmentSY,
            DevelopmentLOS,
            DevelopmentACC,
        }

        public static bool EnableOSD(OSDMode mode = OSDMode.Operational1)
        {
            return CameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdSetOSD_Mode(CameraControl.mav_comm, CameraControl.ackCb, true, (int)mode) == MavProto.mav_error.ok;
        }
        
        public static async Task<bool> EnableOSDAsync(OSDMode mode = OSDMode.Operational1)
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetOSD_Mode(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                }, true, (int)mode);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }
            
            return false;
        }

        public static bool DisableOSD()
        {
            return CameraControlConnected
                && (MavProto.mav_error)MavProto.MavCmdSetOSD_Mode(CameraControl.mav_comm, CameraControl.ackCb, true, (int)OSDMode.Disabled) == MavProto.mav_error.ok;
        }
        
        public static async Task<bool> DisableOSDAsync()
        {
            if (CameraControlConnected)
            {
                TaskCompletionSource<MavProto.MAV_RESULT> CmdAckReceived = new TaskCompletionSource<MavProto.MAV_RESULT>();

                MavProto.mav_error CmdSent = (MavProto.mav_error)MavProto.MavCmdSetOSD_Mode(CameraControl.mav_comm, (command_id, result, unused_arg, handle, buf, buf_len) =>
                {
                    CameraControl.ackCb(command_id, result, unused_arg, handle, buf, buf_len);
                    CmdAckReceived.SetResult((MavProto.MAV_RESULT)result);
                }, true, (int)OSDMode.Disabled);

                return CmdSent == MavProto.mav_error.ok && await CmdAckReceived.Task == MavProto.MAV_RESULT.MAV_RESULT_ACCEPTED;
            }

            return false;
        }

        // ============================== CAMERA CONTROL ==============================
        #endregion
    }
}
