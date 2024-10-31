using MissionPlanner;
using MV04.SingleYaw;
using System;
using System.Windows.Forms;

namespace MV04.TestForms
{
    public partial class SingleYawForm : Form
    {
        private MAVLinkInterface _MAVLink;
        private const double DEG_TO_RAD = Math.PI / 180.0;

        public SingleYawForm(MAVLinkInterface MAVLink)
        {
            InitializeComponent();

            _MAVLink = MAVLink;

            numericUpDown_Kp.Value = (decimal)SingleYawHandler.ControlMulti;
            comboBox_Mode.DataSource = Enum.GetNames(typeof(SingleYawMode));
            comboBox_Mode.SelectedIndex = (int)SingleYawHandler.CurrentMode;

            if (SingleYawHandler.IsRunning)
            {
                button_SwitchSingleYaw.Text = button_SwitchSingleYaw.Text.Replace(" ON", " OFF");
            }

            // Update label on Single-Yaw command event
            SingleYawHandler.SingleYawCommand += (sender, ea) =>
            {
                label_LastSent?.BeginInvoke(new MethodInvoker(() =>
                {
                    if (ea.Mode == SingleYawMode.Master)
                        label_LastSent.Text = $"RC{ea.Channel}Override({Math.Round(ea.Output, 1)})";
                    else if (ea.Mode == SingleYawMode.Slave)
                    {
                        label_LastSent.Text = $"CameraYaw({Math.Round(ea.Output, 1)})";
                    }
                }));
            };

            // Update textbox on Single-Yaw message event
            SingleYawHandler.SingleYawMessage += (sender, ea) => LogLine(ea.Message);
        }

        private void button_SwitchSingleYaw_Click(object sender, EventArgs e)
        {
            if (SingleYawHandler.IsRunning)
            {
                SingleYawHandler.StopSingleYaw();
            }
            else
            {
                SingleYawHandler.StartSingleYaw(_MAVLink, (SingleYawMode)comboBox_Mode.SelectedIndex);
            }

            string buttonStr = SingleYawHandler.IsRunning ? " OFF" : " ON";
            button_SwitchSingleYaw.Text = button_SwitchSingleYaw.Text.Replace(" OFF", "").Replace(" ON", "") + buttonStr;
        }

        private void button_SetYingleYawParams_Click(object sender, EventArgs e)
        {
            SingleYawHandler.ControlMulti = (double)numericUpDown_Kp.Value;
            SingleYawHandler.TestCameraYaw = (int)numericUpDown_CameraYaw.Value;
            SingleYawHandler.ForceTestCameraYaw = checkBox_ForceYaw.Checked;
        }

        public void LogLine(string line)
        {
            textBox_Log?.BeginInvoke(new MethodInvoker(() =>
            {
                if (textBox_Log.Text.Length > 0)
                {
                    textBox_Log.Text += Environment.NewLine;
                }
                textBox_Log.Text += DateTime.Now.ToString("HH:mm:ss - ") + line;

                textBox_Log.SelectionStart = textBox_Log.TextLength;
                textBox_Log.ScrollToCaret();
            }));
        }
    }
}
