using log4net;
using MissionPlanner.Utilities;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class ParamSet2 : UserControl
    {
        /// <summary>
        /// The MAVLink interface
        /// </summary>
        public MAVLinkInterface MAVLink { get; private set; }

        /// <summary>
        /// The ardupilot name of the parameter
        /// </summary>
        public string ParamName { get; private set; }

        private double _ParamToInput = 1;
        private double _InputToParam = 1;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ParamSet2(MAVLinkInterface MAVLink, string paramName)
        {
            InitializeComponent();

            this.MAVLink = MAVLink;
            this.ParamName = paramName;

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
                label2.Visible = false;
                label_ParamValue.Visible = false;
                label3.Visible = false;
                numericUpDown_ParamValue.Visible = false;
                button_Set.Visible = false;

                // Fill out controls
                label_ParamName.Text = $"PARAM \"{ParamName}\" NOT FOUND";
                toolTip_ParamLimits.SetToolTip(this, "Are you connected to the Autopilot?");
                
                log.Error($"Cannot create ParamSet control - Param \"{ParamName}\" not found");
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

                label_ParamName.Text = $"{ParamName} ({unit})";

                double min = 0, max = 0;
                ParameterMetaDataRepository.GetParameterRange(ParamName, ref min, ref max, MainV2.comPort.MAV.cs.firmware.ToString());
                numericUpDown_ParamValue.Minimum = (decimal)Math.Max(double.MinValue, min * _ParamToInput);
                numericUpDown_ParamValue.Maximum = (decimal)Math.Min(double.MaxValue, max * _ParamToInput);

                toolTip_ParamLimits.SetToolTip(this, $"{numericUpDown_ParamValue.Minimum}-{numericUpDown_ParamValue.Maximum} {unit}");

                RefreshCurrentValue();
                numericUpDown_ParamValue.Value = (decimal)Constrain(MAVLink.MAV.param[ParamName].Value * _ParamToInput, (double)numericUpDown_ParamValue.Minimum, (double)numericUpDown_ParamValue.Maximum);

                log.Info($"ParamSet control created for param \"{ParamName}\"");
            }

            this.Invalidate();
        }

        private void RefreshCurrentValue()
        {
            if (MAVLink.MAV.param[ParamName] != null)
            {
                label_ParamValue.Text = (MAVLink.MAV.param[ParamName].Value * _ParamToInput).ToString();
            }
        }

        private double Constrain(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            // Try to send param to UAV
            if (MAVLink.BaseStream == null
                || !MAVLink.BaseStream.IsOpen
                || MAVLink.MAV.param[ParamName] == null
                || !MAVLink.setParam((byte)MAVLink.sysidcurrent, (byte)MAVLink.compidcurrent, ParamName, (double)numericUpDown_ParamValue.Value * _InputToParam))
            {
                CustomMessageBox.Show($"Could not update param {ParamName}", Strings.ERROR);
                log.Error($"Could not update param {ParamName}");
                return;
            }

            // Save value locally
            double oldValue = MAVLink.MAV.param[ParamName].Value;
            MAVLink.MAV.param[ParamName].Value = (double)numericUpDown_ParamValue.Value * _InputToParam;

            // Refresh displayed value
            RefreshCurrentValue();

            log.Info($"Param \"{ParamName}\" value updated ({oldValue} -> {MAVLink.MAV.param[ParamName].Value})");
        }
    }
}
