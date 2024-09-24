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

            comboBox_Dir.SelectedIndex = 0;
            comboBox_Frame.SelectedIndex = 0;

            if (SingleYawHandler.IsRunning)
            {
                button_SwitchSingleYaw.Text = button_SwitchSingleYaw.Text.Replace(" ON", " OFF");
            }

            // Update label on Single-Yaw command event
            SingleYawHandler.SingleYawCommand += (sender, ea) =>
            {
                if (label_LastSent.InvokeRequired)
                {
                    label_LastSent?.Invoke((MethodInvoker)(() => label_LastSent.Text = $"RC{ea.RCChannel}OVERRIDE({ea.RCOutput})"));
                }
                else
                {
                    label_LastSent.Text = $"RC{ea.RCChannel}OVERRIDE({ea.RCOutput})";
                }
            };
        }

        private void button_SendYaw_Click(object sender, EventArgs e)
        {
            // Set combobox parameters
            float Dir;
            switch (comboBox_Dir.SelectedIndex)
            {
                case 0: // CW
                    Dir = 1;
                    break;
                case 1: // CCW
                    Dir = -1;
                    break;
                default: // Auto
                    Dir = 0;
                    break;
            }
            float Frame = (float)comboBox_Frame.SelectedIndex;

            // Send MAV_CMD_CONDITION_YAW
            _MAVLink.doCommand(_MAVLink.MAV.sysid, _MAVLink.MAV.compid, MAVLink.MAV_CMD.CONDITION_YAW,
                (float)numericUpDown_Deg.Value,     // Deg
                (float)numericUpDown_Speed.Value,   // Speed (Deg/s)
                Dir,                                // Dir (1=CW, -1=CCW)
                Frame,                              // Frame (0=Abs, 1=Rel)
                0, 0, 0, false);

            // Raise single-yaw command event
            //SingleYawHandler.TriggerSingleYawCommandEvent(
            //    (int)numericUpDown_Deg.Value,
            //    (int)numericUpDown_Speed.Value,
            //    (int)Dir,
            //    (int)Frame);
        }

        private void button_SendSetPos_Click(object sender, EventArgs e)
        {
            // Set parameters
            float yaw = (float)numericUpDown_Deg.Value * (float)DEG_TO_RAD;
            ushort type_mask = 0b101111111111; // Ingore all except yaw

            //_MAVLink.sendPacket(new MAVLink.mavlink_set_position_target_local_ned_t(
            //    0, 0, 0, 
            //    ), _MAVLink.MAV.sysid, _MAVLink.MAV.compid);
        }

        private void button_SwitchSingleYaw_Click(object sender, EventArgs e)
        {
            if (SingleYawHandler.IsRunning)
            {
                SingleYawHandler.StopSingleYaw();
            }
            else
            {
                SingleYawHandler.StartSingleYaw(_MAVLink);
            }

            string buttonStr = SingleYawHandler.IsRunning ? " OFF" : " ON";
            button_SwitchSingleYaw.Text = button_SwitchSingleYaw.Text.Replace(" OFF", "").Replace(" ON", "") + buttonStr;
        }

        private void button_SetYingleYawParams_Click(object sender, EventArgs e)
        {
            SingleYawHandler.TestCameraYaw = (int)numericUpDown_CameraYaw.Value;
            SingleYawHandler.TestKp = (double)numericUpDown_Kp.Value;
        }
    }
}
