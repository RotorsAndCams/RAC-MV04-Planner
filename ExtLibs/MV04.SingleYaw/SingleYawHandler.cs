using log4net;
using MissionPlanner;
using MissionPlanner.ArduPilot;
using MV04.Camera;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Timers;

namespace MV04.SingleYaw
{
    public class SingleYawCommandEventArgs: EventArgs
    {
        public int Deg { get; set; }

        public int RCChannel { get; set; }

        public int RCOutput { get; set; }
    }

    public static class SingleYawHandler
    {
        #region Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// MAVLink interface this procedure uses to send commands to the UAV
        /// </summary>
        public static MAVLinkInterface MAVLink { get; private set; }

        private static int _YawRCChannel = 7;
        
        private static (short _YawRCMin, short _YawRCTrim, short _YawRCDeadzone, short _YawRCMax, bool _YawRCReversed) _YawRCParams = (1000, 1500, 20, 2000, false);

        public static double TestKp = 1;
        public static int TestCameraYaw = 0;
        public static bool ForceTestCameraYaw = false;

        private static Timer _YawAdjustTimer;

        private static int _YawAdjustInterval = 100; // ms

        private static int _YawAdjustTreshold = 3; // deg

        public static bool IsRunning
        {
            get
            {
                return _YawAdjustTimer != null
                    && _YawAdjustTimer.Enabled;
            }
        }

        public static event EventHandler<SingleYawCommandEventArgs> SingleYawCommand;
        #endregion

        #region Methods
        /// <summary>
        /// Start the yaw correction loop
        /// </summary>
        public static void StartSingleYaw(MAVLinkInterface MAVLink)
        {
            // Set MAVLinkInterface
            SingleYawHandler.MAVLink = MAVLink;

            // Get RC patameters
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
            log.Info($"Single-Yaw timer started (loop={_YawAdjustInterval}ms)");
        }

        private static void _YawAdjustTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Get camera yaw
            float cameraYaw = 0; // positive is CCW
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
            short RCOverride = /*_YawRCParams._YawRCReversed ?*/
                (short)(_YawRCParams._YawRCTrim - (TestKp * (0 - cameraYaw)))/* :
                (short)(_YawRCParams._YawRCTrim + (TestKp * (0 - cameraYaw)))*/;

            // Clamp RCOverride
            if (RCOverride < _YawRCParams._YawRCMin) RCOverride = _YawRCParams._YawRCMin;
            if (RCOverride > _YawRCParams._YawRCMax) RCOverride = _YawRCParams._YawRCMax;

            // Send RCOverride
            MAVLink.MAV.cs.GetType().GetField($"rcoverridech{_YawRCChannel}").SetValue(MAVLink.MAV.cs, RCOverride);

            // Log
            log.Info($"Single-Yaw correction made (RC{_YawRCChannel}={RCOverride})");

            // Raise single-yaw command event
            TriggerSingleYawCommandEvent(
                (int)(cameraYaw * -1), // Flip to UAV frame (positive is CW)
                _YawRCChannel,
                RCOverride);

            /* OLD
            // Check direction
            int Dir = cameraYaw >= 0 ? 1 : -1; // 1=CW, -1=CCW, (0 = Auto)

            // Send MAV_CMD_CONDITION_YAW
            MAVLink.doCommand(MAVLink.MAV.sysid, MAVLink.MAV.compid, global::MAVLink.MAV_CMD.CONDITION_YAW,
                Math.Abs(cameraYaw),    // Deg
                100,                    // Speed (Deg/s)
                Dir,                    // Dir
                1,                      // Abs/Rel (0=Abs, 1=Rel)
                0, 0, 0, false);

            // Raise single-yaw command event
            TriggerSingleYawCommandEvent(
                (int)Math.Round(Math.Abs(cameraYaw)),
                100,
                Dir,
                1);

            // Log
            string turnDir = cameraYaw < 0 ? "CCW" : "CW";
            log.Info($"Single-Yaw correction made ({Math.Abs((int)Math.Round(cameraYaw))}° {turnDir})");
            */
        }

        /// <summary>
        /// Stop the yaw correction loop
        /// </summary>
        public static void StopSingleYaw()
        {
            _YawAdjustTimer.Stop();

            log.Info("Single-Yaw loop stopped");
        }

        /// <summary>
        /// Trigger Single-Yaw Command Event
        /// </summary>
        public static void TriggerSingleYawCommandEvent(int deg, int rcChan, int rcOutput)
        {
            if (SingleYawCommand != null)
            {
                SingleYawCommand(null, new SingleYawCommandEventArgs()
                {
                    Deg = deg,
                    RCChannel = rcChan,
                    RCOutput = rcOutput
                });
            }
        }
        #endregion
    }
}
