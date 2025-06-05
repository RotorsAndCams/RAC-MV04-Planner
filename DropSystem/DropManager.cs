// MissionPlanner/DropSystem/DropManager.cs

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using GMap.NET;
using MissionPlanner;
using MissionPlanner.Utilities;
//using System.Device.Location; // for GeoCoordinate

namespace MissionPlanner.DropSystem
{
    public class DropManager : IDisposable
    {
        // Fixed target position
        public PointLatLng? TargetLocation { get; private set; }

        // Predicted impact point
        public PointLatLng? CurrentImpact {  get; private set; }

        // The position where the drone dropped
        public PointLatLng? ActualDropLocation { get; private set; }

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
            if (_timer.Enabled)
                _timer.Stop();
        }

        // DropNow: is called when the operator decides to drop now
        // Later it will be automated
        public void DropNow()
        {
            if (_hasDropped) return; //Already dropped, ignore
            if (CurrentImpact.HasValue)
            {
                ActualDropLocation = CurrentImpact.Value;
                _hasDropped = true;
                OnDropped?.Invoke(ActualDropLocation.Value);
                TriggerServo(); // DROP
                _timer.Stop();
            }
        }

        // The timer event
        // A method that handles each timer tick
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // no target, do nothing
            if (!TargetLocation.HasValue) return;

            // Read telemetry
            double altAGL = MainV2.comPort.MAV.cs.alt;
            double vx = MainV2.comPort.MAV.cs.vx;
            double vy = MainV2.comPort.MAV.cs.vy;
            // Horizontal speed
            double vHoriz = Math.Sqrt(vx * vx + vy * vy);
            System.Diagnostics.Debug.WriteLine("vHoriz: " + vHoriz);

            double currLat = MainV2.comPort.MAV.cs.lat;
            double currLng = MainV2.comPort.MAV.cs.lng;
            var currentLocation = new PointLatLng(currLat, currLng);
            System.Diagnostics.Debug.WriteLine("currLat: " + currLat);
            System.Diagnostics.Debug.WriteLine("currLng: " + currLng);

            // Compute flying angle
            // Degrees needed, 0 deg = North
            double bearingRad = Math.Atan2(vy, vx);
            double bearingDeg = ((bearingRad * 180.0 / Math.PI) + 360.0) % 360;
            System.Diagnostics.Debug.WriteLine("bearingDeg: " + bearingDeg);

            // Compute impact point
            var impactPoint = DroppingCalculator.ComputeImpactPoint(
                currentLocation,
                altAGL,
                vHoriz,
                bearingDeg);

            CurrentImpact = impactPoint;

            ImpactUpdated?.Invoke(impactPoint);
        }

        public bool CheckRange()
        {
            if (_hasDropped || !CurrentImpact.HasValue || !TargetLocation.HasValue)
                return false;
            double distanceInMeters = DroppingCalculator.HaversineDistance(TargetLocation.Value, CurrentImpact.Value);

            // Acceptable precision radius in meters
            //const double epsilon = 2.0;

            // If close enough -> DROP => return TRUE
            //if (distanceInMeters < epsilonMeters) return true;
            //return false;
            return distanceInMeters <= epsilonMeters;
        }


        // PWM signal to servo
        private void TriggerServo()
        {
            CustomMessageBox.Show("Trigger servo!");
            // Channel 9, pwm 1900
            MainV2.comPort.doCommand(
                MAVLink.MAV_CMD.DO_SET_SERVO,
                9,
                1900,
                0, 0, 0, 0, 0
                );
            // MainV2.comPort.set_servo(9, 1900);
            //MainV2.comPort.doCommand(
            //    MAVLink.MAV_CMD.DO_SET_RELAY,
            //    0, 1, 0, 0, 0, 0, 0 // Relay 0, ON
            //    );

            // Reset after delay
            Task.Delay(1000).ContinueWith(_ =>
            {
                MainV2.comPort.doCommand(
                    MAVLink.MAV_CMD.DO_SET_SERVO,
                    9,
                    1000,
                    0, 0, 0, 0, 0
                    );
                //MainV2.comPort.doCommand(
                //    MAVLink.MAV_CMD.DO_SET_RELAY,
                //    0, 0, 0, 0, 0, 0, 0 // Relay 0, OFF
                //    );
            });
        }

        
        // Dispose (IDisposable)
        public void Dispose()
        {
            _timer.Elapsed -= Timer_Elapsed;
            _timer.Dispose();
        }
    }
}
