using log4net;
using MissionPlanner;
using MissionPlanner.Utilities;
using MV04.Camera;
using System;
using System.Reflection;
using System.Timers;

namespace MV04.SingleYaw
{
    public class SingleYawCommandEventArgs: EventArgs
    {
        public int Deg { get; set; }

        public int RCChannel { get; set; }

        public int RCOutput { get; set; }
    }

    public class SingleYawMessageEventArgs: EventArgs
    {
        public string Message { get; set; }
    }

    public static class SingleYawHandler
    {
        #region Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static int _YawRCChannel = 4;
        
        private static (short _YawRCMin, short _YawRCTrim, short _YawRCDeadzone, short _YawRCMax, bool _YawRCReversed) _YawRCParams = (1000, 1500, 20, 2000, false);


        private static Timer _YawAdjustTimer;

        private static int _YawAdjustInterval = 100; // ms

        private static int _YawAdjustTreshold = 3; // deg

        private static bool _IsConnected
        {
            get
            {
                return MAVLink != null
                    && MAVLink.BaseStream != null
                    && MAVLink.BaseStream.IsOpen
                    && CameraHandler.Instance.IsCameraControlConnected;
            }
        }

        /// <summary>
        /// MAVLink interface this procedure uses to send commands to the UAV
        /// </summary>
        public static MAVLinkInterface MAVLink { get; private set; }

        private static double _ControlMulti = 0;

        /// <summary>
        /// Multiplier applied to the camera yaw degrees applied as PPM to the UAV Yaw RC channel
        /// </summary>
        public static double ControlMulti
        {
            get
            {
                if (_ControlMulti == 0) // Un-initialized
                {
                    if (Settings.Instance.ContainsKey("SINGLE_YAW_CONTROL_MULTI"))
                    {
                        ControlMulti = double.Parse(Settings.Instance["SINGLE_YAW_CONTROL_MULTI"]
                            .Replace(',', (0.1).ToString()[1])
                            .Replace('.', (0.1).ToString()[1]));
                    }
                    else
                    {
                        ControlMulti = 5;
                    }
                    Settings.Instance["SINGLE_YAW_CONTROL_MULTI"] = ControlMulti.ToString();
                }
                return _ControlMulti;
            }
            set
            {
                if (value > 0 && value <= Math.Min(_YawRCParams._YawRCMax - _YawRCParams._YawRCTrim, _YawRCParams._YawRCTrim))
                {
                    _ControlMulti = value;
                }
            }
        }

        /// <summary>
        /// Returns true, if the Single-Yaw loop is running
        /// </summary>
        public static bool IsRunning
        {
            get
            {
                return _YawAdjustTimer != null
                    && _YawAdjustTimer.Enabled;
            }
        }

        /// <summary>
        /// Event triggered on every Single-Yaw command sent
        /// </summary>
        public static event EventHandler<SingleYawCommandEventArgs> SingleYawCommand;

        #region TEST
        public static int TestCameraYaw = 0;
        public static bool ForceTestCameraYaw = false;
        public static event EventHandler<SingleYawMessageEventArgs> SingleYawMessage;
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Start the yaw correction loop
        /// </summary>
        public static void StartSingleYaw(MAVLinkInterface MAVLink)
        {
            // Set MAVLinkInterface
            SingleYawHandler.MAVLink = MAVLink;

            // Check for connections
            if (!_IsConnected)
            {
                return;
            }

            // Get RC parameters
            _YawRCParams = (
                (short)MAVLink.MAV.param[$"RC{_YawRCChannel}_MIN"].Value,
                (short)MAVLink.MAV.param[$"RC{_YawRCChannel}_TRIM"].Value,
                (short)MAVLink.MAV.param[$"RC{_YawRCChannel}_DZ"].Value,
                (short)MAVLink.MAV.param[$"RC{_YawRCChannel}_MAX"].Value,
                (int)MAVLink.MAV.param[$"RC{_YawRCChannel}_REVERSED"].Value == 1
                );

            // Create timer if needed
            if (_YawAdjustTimer == null)
            {
                _YawAdjustTimer = new Timer();
                _YawAdjustTimer.AutoReset = true;
                _YawAdjustTimer.Interval = _YawAdjustInterval;
                _YawAdjustTimer.Elapsed += _YawAdjustTimer_Elapsed;

                log.Info("New Single-Yaw timer object created");
            }

            // Start timer
            _YawAdjustTimer.Start();

            // Log timer start
            log.Info($"Single-Yaw started (loop={_YawAdjustInterval}ms)");

            TriggerSingleYawMessageEvent("Single-Yaw started");
        }

        private static void _YawAdjustTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Check for connections
            if (!_IsConnected)
            {
                return;
            }

            // Get camera yaw
            float cameraYaw = 0; // positive is CW
            if (CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport))
            {
                cameraYaw = ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).roll;
            }
            if (ForceTestCameraYaw)
            {
                cameraYaw = TestCameraYaw;
            }

            // Check if above treshold
            /*if (Math.Abs(cameraYaw) < _YawAdjustTreshold)
            {
                return;
            }*/

            // Calculate RCOverride
            short RCOverride = _YawRCParams._YawRCReversed ?
                (short)(_YawRCParams._YawRCTrim - (ControlMulti * cameraYaw)) : // Reversed
                (short)(_YawRCParams._YawRCTrim + (ControlMulti * cameraYaw));  // Normal

            // Clamp RCOverride
            if (RCOverride < _YawRCParams._YawRCMin) RCOverride = _YawRCParams._YawRCMin;
            if (RCOverride > _YawRCParams._YawRCMax) RCOverride = _YawRCParams._YawRCMax;

            // Set RCOverride
            MAVLink.MAV.cs.GetType().GetField($"rcoverridech{_YawRCChannel}").SetValue(MAVLink.MAV.cs, RCOverride);

            // Log
            log.Info($"Single-Yaw correction made (RC{_YawRCChannel}={RCOverride})");

            // Raise single-yaw command event
            TriggerSingleYawCommandEvent(
                (int)(cameraYaw * -1), // Flip to UAV frame (positive is CW)
                _YawRCChannel,
                RCOverride);

            TriggerSingleYawMessageEvent($"Single-Yaw elapsed (cameraYaw={cameraYaw}, RCChan={_YawRCChannel}, RCOut={RCOverride})");
        }

        /// <summary>
        /// Stop the yaw correction loop
        /// </summary>
        public static void StopSingleYaw()
        {
            // Stop timer
            _YawAdjustTimer.Stop();

            // Send middle stick
            if (_IsConnected)
            {
                MAVLink.MAV.cs.GetType().GetField($"rcoverridech{_YawRCChannel}").SetValue(MAVLink.MAV.cs, _YawRCParams._YawRCTrim);
            }

            log.Info("Single-Yaw stopped");
            TriggerSingleYawCommandEvent(0, _YawRCChannel, _YawRCParams._YawRCTrim);
            TriggerSingleYawMessageEvent("Single-Yaw stopped");
        }

        /// <summary>
        /// Trigger Single-Yaw Command Event
        /// </summary>
        public static void TriggerSingleYawCommandEvent(int deg, int rcChan, int rcOutput)
        {
            SingleYawCommand?.Invoke(null, new SingleYawCommandEventArgs()
            {
                Deg = deg,
                RCChannel = rcChan,
                RCOutput = rcOutput
            });
        }

        public static void TriggerSingleYawMessageEvent(string message)
        {
            SingleYawMessage?.Invoke(null, new SingleYawMessageEventArgs()
            {
                Message = message
            });
        }
        #endregion
    }
}
