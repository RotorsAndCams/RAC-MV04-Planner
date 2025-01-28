using Flurl.Util;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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

        private double _ParamToInput = 1;

        private double _InputToParam = 1;

        public ParamSet(MAVLinkInterface MAVLink, string paramName, string displayName, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            InitializeComponent();

            this.MAVLink = MAVLink;
            this.ParamName = paramName;

            if (MAVLink.MAV.param[ParamName] == null)
            {
                // Return bad param name label instead of control
                this.Controls.Clear();
                this.Height = 40;
                this.Controls.Add(new Label()
                {
                    Text = $"PARAM \"{ParamName}\" NOT FOUND",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(3),
                    Location = new Point(3, 3),
                    Dock = DockStyle.Fill
                });
                toolTip_ValueLimits.SetToolTip(this, "Are you connected to the Autopilot?");
            }
            else
            {
                // Fill out controls
                string unit = ParameterMetaDataRepository.GetParameterMetaData(ParamName, ParameterMetaDataConstants.Units, MainV2.comPort.MAV.cs.firmware.ToString());
                if (string.Equals(unit.ToLower(), "cm"))
                {
                    unit = "m";
                    _InputToParam = 100.0; // m to cm
                    _ParamToInput = 1.0 / _InputToParam; // cm to m
                }

                label_ParamName.Text = $"{displayName} ({unit})";

                double min = 0, max = 0;
                ParameterMetaDataRepository.GetParameterRange(ParamName, ref min, ref max, MainV2.comPort.MAV.cs.firmware.ToString());
                numericUpDown_SetValue.Minimum = (decimal)Math.Max(minValue, min * _ParamToInput);
                numericUpDown_SetValue.Maximum = (decimal)Math.Min(maxValue, max * _ParamToInput);

                toolTip_ValueLimits.SetToolTip(this, $"{numericUpDown_SetValue.Minimum}-{numericUpDown_SetValue.Maximum} {unit}");

                RefreshCurrentValue();
                numericUpDown_SetValue.Value = (decimal)Constrain(MAVLink.MAV.param[ParamName].Value * _ParamToInput, (double)numericUpDown_SetValue.Minimum, (double)numericUpDown_SetValue.Maximum);

                // Resize if name is too long
                if (label_ParamName.Text.Length > 15)
                {
                    this.Height += 13;
                    label_ParamName.Height += 13;
                }
            }
        }

        private void RefreshCurrentValue()
        {
            if (MAVLink.MAV.param[ParamName] != null)
            {
                label_CurrentValue.Text = (MAVLink.MAV.param[ParamName].Value * _ParamToInput).ToString();
            }
        }

        private double Constrain(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void button_SetParam_Click(object sender, EventArgs e)
        {
            // Try to send param to UAV
            if (MAVLink.BaseStream == null
                || !MAVLink.BaseStream.IsOpen
                || MAVLink.MAV.param[ParamName] == null
                || !MAVLink.setParam((byte)MAVLink.sysidcurrent, (byte)MAVLink.compidcurrent, ParamName, (double)numericUpDown_SetValue.Value * _InputToParam))
            {
                CustomMessageBox.Show($"Could not update param {ParamName}", Strings.ERROR);
                return;
            }

            // Save value locally
            MAVLink.MAV.param[ParamName].Value = (double)numericUpDown_SetValue.Value * _InputToParam;

            // Refresh displayed value
            RefreshCurrentValue();
        }
    }
}
