using MissionPlanner;
using MissionPlanner.ArduPilot;
using MV04.SingleYaw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.TestForms
{
    public partial class SingleYawForm : Form
    {
        private MAVLinkInterface _MAVLink;

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
                lock (label_LastSent)
                {
                    label_LastSent.Text = $"MAV_CMD_CONDITION_YAW({ea.Deg}, {ea.Speed}, {ea.Dir}, {ea.Frame})";
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
            SingleYawHandler.TriggerSingleYawCommandEvent(
                (int)numericUpDown_Deg.Value,
                (int)numericUpDown_Speed.Value,
                (int)Dir,
                (int)Frame);
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
    }
}
