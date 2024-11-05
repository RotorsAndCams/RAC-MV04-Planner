using log4net;
using MissionPlanner;
using MissionPlanner.Utilities;
using MV04.Camera;
using MV04.Joystick;
using System;
using System.Reflection;
using System.Timers;

namespace MV04.SingleYaw
{
    public enum SingleYawMode
    {
        None,
        Master,
        Slave,
        Auto
    }

    public class SingleYawCommandEventArgs: EventArgs
    {
        public SingleYawMode Mode { get; set; }

        public double DegError { get; set; }

        public int Channel { get; set; }

        public double Output { get; set; }
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

        private static bool _IsMAVConnected
        {
            get
            {
                return MAVLink != null
                    && MAVLink.BaseStream != null
                    && MAVLink.BaseStream.IsOpen;
            }
        }

        private static bool _IsCameraConnected
        {
            get
            {
                return CameraHandler.Instance.IsCameraControlConnected
                    && CameraHandler.Instance.IsCameraAlive;
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

        private static SingleYawMode _CurrentMode = SingleYawMode.None;

        /// <summary>
        /// Currently running mode
        /// </summary>
        public static SingleYawMode CurrentMode
        {
            get
            {
                if (IsRunning)
                    return _CurrentMode;
                else
                    return SingleYawMode.None;
            }
            private set
            {
                _CurrentMode = value;
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
        public static void StartSingleYaw(MAVLinkInterface MAVLink, SingleYawMode mode = SingleYawMode.Master)
        {
            // Check selected mode
            if (mode == SingleYawMode.None)
            {
                return;
            }
            else if (mode == SingleYawMode.Auto)
            {
                if ((int)MAVLink.MAV.param[$"RCMAP_YAW"].Value == JoystickHandler.GetChannelForJoyRole(MV04_JoyRole.Cam_Yaw)) // 7
                {
                    // Joystick is controlling the camera
                    CurrentMode = SingleYawMode.Master;
                }
                else if ((int)MAVLink.MAV.param[$"RCMAP_YAW"].Value == JoystickHandler.GetChannelForJoyRole(MV04_JoyRole.UAV_Yaw)) // 4
                {
                    // Joystick is controlling the UAV
                    CurrentMode = SingleYawMode.Slave;
                }
                else
                {
                    return;
                }
            }
            else
            {
                CurrentMode = mode;
            }

            // Set MAVLinkInterface
            SingleYawHandler.MAVLink = MAVLink;

            // Check for connection
            if (!_IsMAVConnected)
            {
                return;
            }

            // Get RC parameters
            _YawRCChannel = JoystickHandler.GetChannelForJoyRole(MV04_JoyRole.UAV_Yaw); // 4
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

            // Notify
            log.Info($"Single-Yaw started (loop={_YawAdjustInterval}ms)");
            TriggerSingleYawMessageEvent($"Single-Yaw ({Enum.GetName(typeof(SingleYawMode), CurrentMode)}) started");
        }

        private static void _YawAdjustTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Check for connections & states
            if (!_IsMAVConnected || CurrentMode == SingleYawMode.None)
            {
                return;
            }

            // Get camera yaw
            float cameraYaw = 0; // positive is CW
            if (_IsCameraConnected && CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport))
            {
                cameraYaw = ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).roll;
            }
            if (ForceTestCameraYaw)
            {
                cameraYaw = TestCameraYaw;
            }

            // Correct yaw error according to mode
            if (CurrentMode == SingleYawMode.Master)
            {
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

                // Notify
                log.Info($"Single-Yaw correction made (RC{_YawRCChannel}={RCOverride})");
                TriggerSingleYawCommandEvent(
                    CurrentMode,
                    (int)(cameraYaw),
                    _YawRCChannel,
                    RCOverride);
                TriggerSingleYawMessageEvent($"Single-Yaw elapsed (cameraYaw={cameraYaw}, RCChan={_YawRCChannel}, RCOut={RCOverride})");
            }
            else if (CurrentMode == SingleYawMode.Slave)
            {
                // Check if above treshold
                /*if (Math.Abs(cameraYaw) < _YawAdjustTreshold)
                {
                    CameraHandler.Instance.SetCameraYaw(YawDirection.Stop, 0);
                    return;
                }*/

                // Calculate camera yaw
                YawDirection yawDirection = cameraYaw > 0 ? YawDirection.Left : YawDirection.Right;
                double yawSpeed = Math.Abs(cameraYaw) / 180.0; // Map 0-180 to 0.0-1.0

                // Set camera yaw
                CameraHandler.Instance.SetCameraYaw(yawDirection, (float)yawSpeed);

                // Notify
                log.Info($"Single-Yaw correction made ({Enum.GetName(typeof(YawDirection), yawDirection)} {Math.Round(yawSpeed, 1)})");
                TriggerSingleYawCommandEvent(
                    CurrentMode,
                    (int)(cameraYaw),
                    _YawRCChannel,
                    yawDirection == YawDirection.Right ? yawSpeed : yawSpeed * -1);
                TriggerSingleYawMessageEvent($"Single-Yaw elapsed (Camera Yaw {Enum.GetName(typeof(YawDirection), yawDirection)} {Math.Round(yawSpeed, 1)})");
            }
        }


        /// <summary>
        /// Stop the yaw correction loop
        /// </summary>
        public static void StopSingleYaw()
        {
            // Stop timer
            _YawAdjustTimer.Stop();

            if (CurrentMode == SingleYawMode.Master)
            {
                // Send middle stick
                MAVLink.MAV.cs.GetType().GetField($"rcoverridech{_YawRCChannel}").SetValue(MAVLink.MAV.cs, _YawRCParams._YawRCTrim);

                // Notify
                TriggerSingleYawCommandEvent(SingleYawMode.Master, 0, _YawRCChannel, _YawRCParams._YawRCTrim);
            }
            else if (CurrentMode == SingleYawMode.Slave)
            {
                // Stop gimbal
                CameraHandler.Instance.SetCameraYaw(YawDirection.Stop, 0);

                // Notify
                TriggerSingleYawCommandEvent(SingleYawMode.Slave, 0, 0, 0);
            }

            // Reset mode
            CurrentMode = SingleYawMode.None;

            // Notify
            log.Info("Single-Yaw stopped");
            TriggerSingleYawMessageEvent("Single-Yaw stopped");
        }

        /// <summary>
        /// Trigger Single-Yaw Command Event
        /// </summary>
        public static void TriggerSingleYawCommandEvent(SingleYawMode mode, double deg, int chan, double output)
        {
            SingleYawCommand?.Invoke(null, new SingleYawCommandEventArgs()
            {
                Mode = mode,
                DegError = deg,
                Channel = chan,
                Output = output
            });
        }

        /// <summary>
        /// Trigger Single-Yaw Message Event
        /// </summary>
        /// <param name="message">Message to pass as event argument</param>
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
