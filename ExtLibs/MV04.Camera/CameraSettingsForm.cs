using MissionPlanner.Utilities;
using MV04.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Camera
{
    public partial class CameraSettingsForm : Form
    {
        private const int CHANGE_WINDOW_WIDTH = 225;
        private const int CHANGE_WINDOW_HEIGHT = 360;
        private static CameraSettingsForm _instance;

        public static CameraSettingsForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CameraSettingsForm();
                return _instance;
            }
        }

        private CameraSettingsForm()
        {
            InitializeComponent();

            this.KeyPreview = true;

            _isRecording = bool.Parse(SettingManager.Get(Setting.AutoRecordVideoStream));
            SetButtonStatus();
        }


        private void btn_DayCamera_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.SetImageSensor(false); //Set to Day camera
        }

        private void btn_NightCamera_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.SetImageSensor(true); //Set to Night camera
        }

        private void btn_NUC_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.DoNUC();
        }

        bool isReconnecting = false;

        private async void btn_Reconnect_Click(object sender, EventArgs e)
        {
            if (isReconnecting)
                return;

            this.btn_Reconnect.Enabled = false;

            await Task.Run(() => {
                DoReconnect();
            });

            this.btn_Reconnect.Enabled = true;
        }

        private void DoReconnect()
        {
            try
            {
                isReconnecting = true;
                CameraHandler.Instance.StartGstreamer(CameraHandler.url);
                CameraHandler.Instance.CameraControlConnect(
                    IPAddress.Parse(SettingManager.Get(Setting.CameraIP)),
                    int.Parse(SettingManager.Get(Setting.CameraControlPort)));

                isReconnecting = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not reconnect to camera: " + ex.Message);
                isReconnecting = false;
            }
        }

        public event EventHandler event_ReconnectRequested;
        public event EventHandler event_StartStopRecording;


        private void btn_StartStopRecording_Click(object sender, EventArgs e)
        {
            if (event_StartStopRecording != null)
                event_StartStopRecording(sender, e);
        }

        bool _isRecording;

        public void SetRecordingStatus(bool status)
        {
            _isRecording = status;

            if (InvokeRequired)
                Invoke(new Action(() => { SetButtonStatus(); }));
            else
            {
                SetButtonStatus();
            }
        }

        private void SetButtonStatus()
        {
            if (_isRecording)
                this.btn_StartStopRecording.ForeColor = Color.Red;
            else
                this.btn_StartStopRecording.ForeColor = Color.White;
        }

    }
}
