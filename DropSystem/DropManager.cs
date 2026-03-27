// MissionPlanner/DropSystem/DropManager.cs

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
//using System.Device.Location; // for GeoCoordinate

namespace MissionPlanner.DropSystem
{
    public class DropManager : IDisposable
    {
        private Stopwatch _cycleStopwatch = new Stopwatch();
        private int _cycleCounter = 0;
        private DateTime _lastLogTime = DateTime.Now;


        // Fixed target position
        public PointLatLng? TargetLocation { get; private set; }

        // Predicted impact point
        public PointLatLng? CurrentImpact {  get; private set; }

        // The position where the drone dropped
        public PointLatLng? ActualDropLocation { get; private set; }

        // The next position we want to drop to
        public PointLatLng? NextTarget { get; private set; }
        public float NextTargetAlt {  get; private set; }

        // Fired whenever CurrentImpact is updated
        public event Action<PointLatLng> ImpactUpdated;

        // Fired whenever drop actually happens
        public event Action<PointLatLng> OnDropped;

        // Timer
        private readonly Timer _timer;

        // Epsilon range for precision drop (in meters)
        double epsilonMeters = 2.0;

        // Private, don't want to change it from outside, BUT we need to read it outside: HasDropped function can reach it for readonly
        private bool _hasDropped = false;
        public bool HasDropped => _hasDropped;

        // Servo channel
        private int _servoChannel = 9;


        public DropManager()
        {
            _timer = new Timer(200);
            _timer.Elapsed += Timer_Elapsed;
            _timer.AutoReset = true;
        }

        public void SetTarget(PointLatLng target)
        {
            TargetLocation = target;
            CurrentImpact = null;
            ActualDropLocation = null;
        }

        // Start: starts the internal timer thus ImpactUpdated events fire every 200 ms
        public void Start()
        {

            _hasDropped = false;
            if (!_timer.Enabled)
                _timer.Start();
        }

        // Stop: stop the impact calculation
        public void Stop()
        {
            try
            {
                if (_timer != null)
                {
                    _timer.Stop();
                }
            }
            catch {  }
        }

        // DropNow: is called when the operator decides to drop now
        // Later it will be automated
        public void DropNow()
        {
            if (_hasDropped)
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

        // The timer event
        // A method that handles each timer tick
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_hasDropped)
                return;

            // no target, do nothing
            if (!NextTarget.HasValue) return;

            if (MainV2.comPort.MAV.cs.mode.ToUpper() != "GUIDED")
                MainV2.comPort.setMode((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, "GUIDED");


            // Getting current drone position
            double currLat = MainV2.comPort.MAV.cs.lat;
            double currLng = MainV2.comPort.MAV.cs.lng;
            var currentLocation = new PointLatLng(currLat, currLng);

            // Calculate bearing and offset
            double bearing = DroppingCalculator.Bearing(currentLocation, NextTarget.Value);
            double offset = 10; // UAV fly through the drop target position
            var actualWaypoint = DroppingCalculator.OffsetPoint(NextTarget.Value, offset, bearing);

            // 4) Send that as a repeated guided waypoint
            var wp = new Locationwp
            {
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT,
                alt = NextTargetAlt,
                lat = actualWaypoint.Lat,
                lng = actualWaypoint.Lng
            };
            MainV2.comPort.setGuidedModeWP(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                wp);

            // Read telemetry
            double altAGL = MainV2.comPort.MAV.cs.alt;
            double vx = MainV2.comPort.MAV.cs.vx;
            double vy = MainV2.comPort.MAV.cs.vy;
            // Horizontal speed
            double vHoriz = Math.Sqrt(vx * vx + vy * vy);
            
            // Compute flying angle
            // Degrees needed, 0 deg = North
            double bearingRad = Math.Atan2(vy, vx);
            double bearingDeg = ((bearingRad * 180.0 / Math.PI) + 360.0) % 360;

            // Compute impact point
            var impactPoint = DroppingCalculator.ComputeImpactPoint(
                currentLocation,
                altAGL,
                vHoriz,
                bearingDeg);

            CurrentImpact = impactPoint;


            //DROPNOW - INVOKE
            ImpactUpdated?.Invoke(impactPoint);

            if (!_hasDropped)
            {
                if (CheckRange())
                {
                    DropNow();
                }
            }

        }

        public bool CheckRange()
        {
            if (_hasDropped || !CurrentImpact.HasValue || !NextTarget.HasValue)
                return false;
            double distanceInMeters = DroppingCalculator.HaversineDistance(NextTarget.Value, CurrentImpact.Value);
            System.Diagnostics.Debug.WriteLine($"[DropManager] Distance to target: {distanceInMeters} m");

            //quadratic error range tolerance -> larger precision error at higher speed
            double velocityQuadraticOffset = 0.05 * MainV2.comPort.MAV.cs.groundspeed * MainV2.comPort.MAV.cs.groundspeed;


            System.Diagnostics.Debug.WriteLine($"[DropManager] epsilonMeters + offset: {epsilonMeters + Math.Min(5.0, velocityQuadraticOffset)} m");
            return distanceInMeters <= (epsilonMeters + Math.Min(5.0, velocityQuadraticOffset));
        }

        public void TriggerServo(int p_ServoChannel)
        {
            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                p_ServoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                p_ServoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);


            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                p_ServoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            // Reset after delay
            //Task.Delay(1000).ContinueWith(_ =>
            //{
            //    MainV2.comPort.doCommand(
            //    (byte)MainV2.comPort.sysidcurrent,
            //    (byte)MainV2.comPort.compidcurrent,
            //    MAVLink.MAV_CMD.DO_SET_SERVO,
            //    p_ServoChannel,     // servo number
            //    1900,  // pwm value
            //    0, 0, 0, 0, 0);

            //});
        }


        // PWM signal to servo
        public void TriggerServo()
        {
            // Channel 9, pwm 1900
            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                _servoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                _servoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            MainV2.comPort.doCommand(
                (byte)MainV2.comPort.sysidcurrent,
                (byte)MainV2.comPort.compidcurrent,
                MAVLink.MAV_CMD.DO_SET_SERVO,
                _servoChannel,     // servo number
                (int)1100,  // pwm value
                0, 0, 0, 0, 0);

            // Reset after delay
            //Task.Delay(1000).ContinueWith(_ =>
            //{
            //    MainV2.comPort.doCommand(
            //    (byte)MainV2.comPort.sysidcurrent,
            //    (byte)MainV2.comPort.compidcurrent,
            //    MAVLink.MAV_CMD.DO_SET_SERVO,
            //    _servoChannel,     // servo number
            //    1900,  // pwm value
            //    0, 0, 0, 0, 0);

            //});
            _hasDropped = true;

            if(_timer != null)
            {
                _timer.Stop();
            }
            
        }

        // Set servo channel
        public void SetServoChannel(int channel)
        {
            _servoChannel = channel;
        }

        public void SetNextTarget(PointLatLng pt, float alt)
        {
            NextTarget = pt;
            NextTargetAlt = alt;
            _hasDropped = false;
            Start();
        }

        
        // Dispose (IDisposable)
        public void Dispose()
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();
        }
    }
}
