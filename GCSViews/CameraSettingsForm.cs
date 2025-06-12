using MV04.Camera;
using MV04.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class CameraSettingsForm : Form
    {
        private const int CHANGE_WINDOW_WIDTH = 225;
        private const int CHANGE_WINDOW_HEIGHT = 360;
        private static CameraSettingsForm _instance;
        bool isReconnecting = false;
        private bool buttonDown;
        public event EventHandler event_StartStopRecording;
        bool _isRecording;
        private CancellationTokenSource EmergencyStopTaskCTS;
        private bool _IsDialogOpen = false;

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

        private async void btn_Reconnect_Click(object sender, EventArgs e)
        {
            if (isReconnecting)
                return;

            MainV2.instance.SwitchTRIPRelay(true);

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
                
                CameraHandler.Instance.CameraControlConnect(
                    IPAddress.Parse(SettingManager.Get(Setting.CameraIP)),
                    int.Parse(SettingManager.Get(Setting.CameraControlPort)));
                Thread.Sleep(2000);

                isReconnecting = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not reconnect to camera: " + ex.Message);
                isReconnecting = false;
            }
        }

        private void btn_StartStopRecording_Click(object sender, EventArgs e)
        {
        }

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

        private void EmergencyStop()
        {
            bool b = MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.DO_FLIGHTTERMINATION, 1, 0, 0, 0, 0, 0, 0);
            MessageBox.Show(b ? "Emergency stop was succesful." : "Failed to emergency stop.");
        }

        private void btn_EmergencyStop_MouseDown(object sender, MouseEventArgs e)
        {
            if (_IsDialogOpen)
                return;

            lb_StopCounter.Visible = true;
            btn_EmergencyStop.BackColor = Color.Red;

            EmergencyStopTaskCTS = new CancellationTokenSource();
            Task.Factory.StartNew(() => { ButtonHoldMethod(); }, EmergencyStopTaskCTS.Token);
        }

        private void ButtonHoldMethod()
        {
            int num = 3;
            System.Threading.Thread.Sleep(500);
            buttonDown = true;

            do
            {
                if (EmergencyStopTaskCTS.Token.IsCancellationRequested)
                {
                    return;
                }

                if (InvokeRequired)
                    Invoke(new Action(() => { lb_StopCounter.Text = "Motor stop " + num; }));
                else
                    lb_StopCounter.Text = "Motor stop " + num;

                if (num <= 0 && !_IsDialogOpen)
                {
                    #region Ask are u sure

                    _IsDialogOpen = true;

                    DialogResult dialogResult = MessageBox.Show("The copter will stop the rotors!\nIt can cause damege to the vehicle!\n\nAre you Sure?", "Emergency stop", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        EmergencyStop();
                        _IsDialogOpen = false;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        buttonDown = false;

                        if (InvokeRequired)
                            Invoke(new Action(() => { lb_StopCounter.Visible = false; }));
                        else
                            lb_StopCounter.Visible = false;

                        _IsDialogOpen = false;

                        break;
                    }

                    #endregion
                }

                System.Threading.Thread.Sleep(1000);

                num--;

            } while (buttonDown);

            if (InvokeRequired)
                Invoke(new Action(() => { lb_StopCounter.Text = "Motor stop " + 3; }));
            else
                lb_StopCounter.Text = "Motor stop " + 3;
        }

        private void btn_EmergencyStop_MouseUp(object sender, MouseEventArgs e)
        {
            buttonDown = false;
            lb_StopCounter.Visible = false;
            btn_EmergencyStop.BackColor = Color.Black;
            EmergencyStopTaskCTS.Cancel();
        }
    }
}
