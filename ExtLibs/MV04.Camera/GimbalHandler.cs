using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MV04.Camera
{
    public class GimbalHandler
    {
        #region Fields

        private GimbalHandler _instance;

        public GimbalHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GimbalHandler();

                Camera = CameraHandler.Instance;
                GimbalTimer = new System.Windows.Forms.Timer();

                return _instance;
            }
        }

        private CameraHandler Camera;

        private System.Windows.Forms.Timer GimbalTimer;

        #endregion

        #region Methods

        private GimbalHandler(){} // Hide ctor

        public void StartGimbal(TimeSpan? timerInterval)
        {
            if (GimbalTimer.Enabled) GimbalTimer.Stop();

            if (!timerInterval.HasValue) timerInterval = TimeSpan.FromMilliseconds(100); // Default gimbal update frequency is 10Hz
            GimbalTimer.Interval = (int)Math.Round(timerInterval.Value.TotalMilliseconds);
            GimbalTimer.Start();
        }

        private void GimbalTimer_Tick(object sender, EventArgs e)
        {
            Camera.MoveCamera(0, 0, 0); // TODO: Send new inputs
        }

        public void StopGimbal()
        {
            GimbalTimer.Stop();
        }



        #endregion
    }
}
