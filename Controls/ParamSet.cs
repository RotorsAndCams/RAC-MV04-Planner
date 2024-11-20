using Flurl.Util;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace MissionPlanner.Controls
{
    public partial class ParamSet : UserControl
    {
        public MAVLinkInterface MAVLink { get; private set; }

        public string ParamName { get; private set; }

        public double MinValue { get; private set; }

        public double MaxValue { get; private set; }

        public ParamSet(MAVLinkInterface MAVLink, string paramName, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            InitializeComponent();

            this.MAVLink = MAVLink;
            this.ParamName = paramName;

            if (MAVLink.MAV.param[ParamName] == null)
            {
                // Return bad param name label instead of control
                this.Controls.Clear();
                //this.Controls.Add(new Label()
                //{
                //    Text = $"PARAM \"{ParamName}\" NOT FOUND",
                //    TextAlign = ContentAlignment.MiddleLeft,
                //    ForeColor = Color.Red,
                //    Margin = new Padding(3),
                //    Location = new Point(3, 3),
                //    Dock = DockStyle.Fill,
                //    AutoSize = true
                //});
                toolTip_ValueLimits.SetToolTip(this, "Are you connected to the Autopilot?");
            }
            else
            {
                // Fill out controls
                label_ParamName.Text = ParamName;

                double min = 0, max = 0;
                ParameterMetaDataRepository.GetParameterRange(ParamName, ref min, ref max, MainV2.comPort.MAV.cs.firmware.ToString());
                MinValue = Math.Max(minValue, min);
                MaxValue = Math.Min(maxValue, max);
                numericUpDown_SetValue.Minimum = (decimal)MinValue;
                numericUpDown_SetValue.Maximum = (decimal)MaxValue;

                label_Unit1.Text = ParameterMetaDataRepository.GetParameterMetaData(ParamName, ParameterMetaDataConstants.Units, MainV2.comPort.MAV.cs.firmware.ToString());
                label_Unit2.Text = label_Unit1.Text;
                toolTip_ValueLimits.SetToolTip(this, $"{min}-{max} {label_Unit1.Text}");

                RefreshCurrentValue();
                numericUpDown_SetValue.Value = (decimal)MAVLink.MAV.param[ParamName].Value;
            }
        }

        private void RefreshCurrentValue()
        {
            if (MAVLink.MAV.param[ParamName] != null)
            {
                label_CurrentValue.Text = MAVLink.MAV.param[ParamName].ToString();
            }
        }

        private void button_SetParam_Click(object sender, EventArgs e)
        {
            // Try to send param to UAV
            if (MAVLink.BaseStream == null
                || !MAVLink.BaseStream.IsOpen
                || MAVLink.MAV.param[ParamName] == null
                || (double)numericUpDown_SetValue.Value < MinValue
                || (double)numericUpDown_SetValue.Value > MaxValue
                || !MAVLink.setParam((byte)MAVLink.sysidcurrent, (byte)MAVLink.compidcurrent, ParamName, (double)numericUpDown_SetValue.Value))
            {
                CustomMessageBox.Show($"Could not update param {ParamName}", Strings.ERROR);
                return;
            }

            // Save value locally
            MAVLink.MAV.param[ParamName].Value = (double)numericUpDown_SetValue.Value;

            // Refresh displayed value
            RefreshCurrentValue();
        }
    }
}
