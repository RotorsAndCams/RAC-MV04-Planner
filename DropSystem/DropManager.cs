using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.UI.WebControls;
using GMap.NET;
using MissionPlanner;
using MissionPlanner.Utilities;
using System.Diagnostics;

namespace MissionPlanner.DropSystem
{
    public class DropManager
    {
        #region Events

        // Fired whenever CurrentImpact is updated
        public event Action<PointLatLng> ImpactUpdated;
        // Fired whenever drop actually happens
        public event Action<PointLatLng> OnDropped;

        #endregion

        #region Props

        /// <summary>
        /// Predicted impact point
        /// </summary>
        public PointLatLng? CurrentImpact {  get; private set; }

        /// <summary>
        /// The position where the drone dropped
        /// </summary>
        public PointLatLng? ActualDropLocation { get; private set; }

        /// <summary>
        /// The next position we want to drop to
        /// </summary>
        public PointLatLng? NextTarget { get; private set; }
        public float NextTargetAlt {  get; private set; }

        public int ServoChannel { get; set; } = 6;

        public bool HasDropped { get; private set; } = false;

        #endregion

        #region Fields

        private readonly Timer _timer;

        // Epsilon range for precision drop (in meters)
        private double epsilonMeters = 2.0;

        #endregion

        #region Ctor

        public DropManager()
        {
            _timer = new Timer(200);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        #endregion

        #region Methods

        private void Start()
        {

            HasDropped = false;
            if (!_timer.Enabled)
                _timer.Start();
        }

        public void Stop()
        {
            try
            {
                if (_timer != null)
                {
                    _timer.Stop();
                }
            }
            catch { }
        }

        /// <summary>
        /// This Set the target and start the drop process timer
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="alt"></param>
        public void SetNextTarget(PointLatLng pt, float alt)
        {
            Stop();
            NextTarget = pt;
            NextTargetAlt = alt;
            HasDropped = false;
            Start();
        }

        //1. step
        public bool CheckRange()
        {
            if (HasDropped || !CurrentImpact.HasValue || !NextTarget.HasValue)
                return false;
            double distanceInMeters = DroppingCalculator.HaversineDistance(NextTarget.Value, CurrentImpact.Value);
            System.Diagnostics.Debug.WriteLine($"[DropManager] Distance to target: {distanceInMeters} m");

            //quadratic error range tolerance -> larger precision error at higher speed
            double velocityQuadraticOffset = 0.05 * MainV2.comPort.MAV.cs.groundspeed * MainV2.comPort.MAV.cs.groundspeed;

            System.Diagnostics.Debug.WriteLine($"[DropManager] epsilonMeters + offset: {epsilonMeters + Math.Min(5.0, velocityQuadraticOffset)} m");
            return distanceInMeters <= (epsilonMeters + Math.Min(5.0, velocityQuadraticOffset));
        }

        //2. step
        public void DropNow()
        {
            if (HasDropped)
            {
                System.Diagnostics.Debug.WriteLine("Bomb has already dropped!");
                return; //Already dropped, ignore
            }
            if (CurrentImpact.HasValue)
            {
                ActualDropLocation = CurrentImpact.Value;
                OnDropped?.Invoke(ActualDropLocation.Value);

                TriggerServo();
            }
        }

        //3. step
        public void TriggerServo()
        {
            // Channel 9, pwm 1900
            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                ServoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                ServoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                ServoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            HasDropped = true;

            if (_timer != null)
                _timer.Stop();
        }

        #endregion

        #region EventHandlers

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            #region error handling

            if (HasDropped)
                return;

            if (!NextTarget.HasValue)
                return;

            #endregion

            #region Set to guided mode

            if (MainV2.comPort.MAV.cs.mode.ToUpper() != "GUIDED")
                MainV2.comPort.setMode((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, "GUIDED");

            #endregion

            #region Calculate bearing and offset

            var currentLocation = new PointLatLng(MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng);

            double bearing = DroppingCalculator.Bearing(currentLocation, NextTarget.Value);
            double offset = 10; // UAV fly through the drop target position
            var actualWaypoint = DroppingCalculator.OffsetPoint(NextTarget.Value, offset, bearing);

            #endregion

            #region Send to waypoint

            var wp = new Locationwp
            {
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT,
                alt = NextTargetAlt,
                lat = actualWaypoint.Lat,
                lng = actualWaypoint.Lng
            };
            MainV2.comPort.setGuidedModeWP_NoErrorMSG(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                wp);

            #endregion

            #region Read telemetry

            double altAGL = MainV2.comPort.MAV.cs.alt;
            double vx = MainV2.comPort.MAV.cs.vx;
            double vy = MainV2.comPort.MAV.cs.vy;
            // Horizontal speed
            double vHoriz = Math.Sqrt(vx * vx + vy * vy);

            #endregion

            #region Compute impact point

            // Compute flying angle - Degrees needed, 0 deg = North
            double bearingRad = Math.Atan2(vy, vx);
            double bearingDeg = ((bearingRad * 180.0 / Math.PI) + 360.0) % 360;

            // 
            var impactPoint = DroppingCalculator.ComputeImpactPoint(
                currentLocation,
                altAGL,
                vHoriz,
                bearingDeg);

            CurrentImpact = impactPoint;

            //DROPNOW - INVOKE
            ImpactUpdated?.Invoke(impactPoint);

            #endregion

            if (!HasDropped)
            {
                if (CheckRange())
                {
                    DropNow();
                }
            }

        }

        #endregion

    }
}
