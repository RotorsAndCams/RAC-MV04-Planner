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
        /// <summary>
        /// The MAVLink interface
        /// </summary>
        public MAVLinkInterface MAVLink { get; private set; }

        /// <summary>
        /// The ardupilot name of the parameter
        /// </summary>
        public string ParamName { get; private set; }

        /// <summary>
        /// The display name of the parameter
        /// </summary>
        public string DisplayName { get; private set; }

        private double _InputMinValue = double.MinValue;
        private double _InputMaxValue = double.MaxValue;
        private double _ParamToInput = 1;
        private double _InputToParam = 1;

        public ParamSet(MAVLinkInterface MAVLink, string paramName, string displayName, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            InitializeComponent();

            this.MAVLink = MAVLink;
            this.ParamName = paramName;
            this.DisplayName = displayName;
            this._InputMinValue = minValue;
            this._InputMaxValue = maxValue;

            Reload();
        }

        /// <summary>
        /// Udpates the control appearance based on the current connection status
        /// </summary>
        public void Reload()
        {
            if (MAVLink.MAV.param[ParamName] == null)
            {
                // Set control visibility (not connected)
                label1.Visible = false;
                label_CurrentValue.Visible = false;
                label2.Visible = false;
                numericUpDown_SetValue.Visible = false;
                button_SetParam.Visible = false;

                // Fill out controls
                label_ParamName.Text = $"PARAM \"{ParamName}\" NOT FOUND";
                label_ParamName.Height = 13 * (int)Math.Ceiling(label_ParamName.Text.Length / 16.0);
                toolTip_ValueLimits.SetToolTip(this, "Are you connected to the Autopilot?");
            }
            else
            {
                // Set control visibility (connected)
                foreach (Control item in tableLayoutPanel1.Controls)
                {
                    item.Visible = true;
                }

                // Fill out controls
                string unit = ParameterMetaDataRepository.GetParameterMetaData(ParamName, ParameterMetaDataConstants.Units, MainV2.comPort.MAV.cs.firmware.ToString());
                if (string.Equals(unit.ToLower(), "cm"))
                {
                    unit = "m";
                    _InputToParam = 100.0; // m to cm
                    _ParamToInput = 1.0 / _InputToParam; // cm to m
                }

                label_ParamName.Text = $"{DisplayName} ({unit})";
                label_ParamName.Height = 13 * (int)Math.Ceiling(label_ParamName.Text.Length / 16.0);

                double min = 0, max = 0;
                ParameterMetaDataRepository.GetParameterRange(ParamName, ref min, ref max, MainV2.comPort.MAV.cs.firmware.ToString());
                numericUpDown_SetValue.Minimum = (decimal)Math.Max(_InputMinValue, min * _ParamToInput);
                numericUpDown_SetValue.Maximum = (decimal)Math.Min(_InputMaxValue, max * _ParamToInput);

                toolTip_ValueLimits.SetToolTip(this, $"{numericUpDown_SetValue.Minimum}-{numericUpDown_SetValue.Maximum} {unit}");

                RefreshCurrentValue();
                numericUpDown_SetValue.Value = (decimal)Constrain(MAVLink.MAV.param[ParamName].Value * _ParamToInput, (double)numericUpDown_SetValue.Minimum, (double)numericUpDown_SetValue.Maximum);
            }

            this.Invalidate();
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
