using log4net;
using MissionPlanner;
using MV04.Camera;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Timers;

namespace MV04.SingleYaw
{
    public static class SingleYawHandler
    {
        #region Fields
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// MAVLink interface this procedure uses to send commands to the UAV
        /// </summary>
        public static MAVLinkInterface MAVLink { get; private set; }

        private static Timer _YawAdjustTimer;

        private static int _YawAdjustInterval = 100; // ms

        private static int _YawAdjustTreshold = 3; // deg
        #endregion

        #region Methods
        private static void _YawAdjustTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Get camera yaw
            float cameraYaw = 0;
            if (CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport))
            {
                cameraYaw = ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).roll;
            }

            // Check if above treshold
            if (Math.Abs(cameraYaw) < _YawAdjustTreshold)
            {
                return;
            }

            // Send MAV_CMD_CONDITION_YAW
            MAVLink.doCommand(MAVLink.MAV.sysid, MAVLink.MAV.compid, global::MAVLink.MAV_CMD.CONDITION_YAW,
                cameraYaw,  // Deg
                100,        // Speed (Deg/s)
                -1,         // Dir (1=CW, -1=CCW) -> CCW matches camera report yaw direction
                1,          // Abs/Rel (0=Abs, 1=Rel)
                0, 0, 0, false);

            // Log
            string turnDir = cameraYaw < 0 ? "CCW" : "CW";
            log.Info($"Single-Yaw correction made ({Math.Abs((int)Math.Round(cameraYaw))}° {turnDir})");
        }

        /// <summary>
        /// Start the yaw correction loop
        /// </summary>
        public static void StartSingleYaw(MAVLinkInterface MAVLink)
        {
            SingleYawHandler.MAVLink = MAVLink;

            if (_YawAdjustTimer == null)
            {
                _YawAdjustTimer = new Timer();
                _YawAdjustTimer.AutoReset = true;
                _YawAdjustTimer.Interval = _YawAdjustInterval;
                _YawAdjustTimer.Elapsed += _YawAdjustTimer_Elapsed;

                log.Info("New Single-Yaw timer object created");
            }

            _YawAdjustTimer.Start();

            log.Info($"Single-Yaw loop started (loop: {_YawAdjustInterval}ms, treshold: ±{_YawAdjustTreshold}°)");
        }

        /// <summary>
        /// Stop the yaw correction loop
        /// </summary>
        public static void StopSingleYaw()
        {
            _YawAdjustTimer.Stop();

            log.Info("Single-Yaw loop stopped");
        }
        #endregion
    }
}
