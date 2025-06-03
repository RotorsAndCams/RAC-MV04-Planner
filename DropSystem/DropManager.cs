// MissionPlanner/DropSystem/DropManager.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Timers;
using GMap.NET;
using MissionPlanner;
using MissionPlanner.Utilities;

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
            if (CurrentImpact.HasValue)
            {
                ActualDropLocation = CurrentImpact.Value;
                OnDropped?.Invoke(ActualDropLocation.Value);
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
            //double vx = CurrentState.vx;


        }
         






        public void Dispose()
        {
            //_timer.Elapsed -= Timer_Elapsed;
            //_timer.Dispose();
        }
    }

}
