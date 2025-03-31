using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MV04.Settings
{
    public partial class SettingForm : Form
    {
        internal HashSet<SettingItem> returnData;

        internal SettingForm(HashSet<SettingItem> formData)
        {
            InitializeComponent();
            this.BringToFront();

            // Set UI from formData
            textBox_cameraIp.Text = GetValue(formData, Setting.CameraIP);
            textBox_cameraControlPort.Text = GetValue(formData, Setting.CameraControlPort);
            textBox_StreamUrl.Text = GetValue(formData, Setting.CameraStreamUrl);
            radioButton_AutoConnect_Yes.Checked = bool.Parse(GetValue(formData, Setting.AutoConnect));
            radioButton_AutoConnect_No.Checked = !radioButton_AutoConnect_Yes.Checked;
            numericUpDown_VideoSegmentLength.Value = int.Parse(GetValue(formData, Setting.VideoSegmentLength));
            comboBox_IrColorMode.SelectedItem = GetValue(formData, Setting.IrColorMode);
            comboBox_coordFormat.SelectedItem = GetValue(formData, Setting.GPSType);
            comboBox_altFormat.SelectedItem = GetValue(formData, Setting.AltFormat);
            comboBox_distFormat.SelectedItem = GetValue(formData, Setting.DistFormat);
            comboBox_speedFormat.SelectedItem = GetValue(formData, Setting.SpeedFormat);

            returnData = formData;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            // Validate & save contents
            SetIfValid(returnData, Setting.CameraIP, textBox_cameraIp.Text);
            SetIfValid(returnData, Setting.CameraControlPort, textBox_cameraControlPort.Text);
            SetIfValid(returnData, Setting.CameraStreamUrl, textBox_StreamUrl.Text);
            SetIfValid(returnData, Setting.AutoConnect, radioButton_AutoConnect_Yes.Checked.ToString());
            SetIfValid(returnData, Setting.VideoSegmentLength, numericUpDown_VideoSegmentLength.Value.ToString());
            SetIfValid(returnData, Setting.IrColorMode, comboBox_IrColorMode.SelectedItem.ToString());
            SetIfValid(returnData, Setting.GPSType, comboBox_coordFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.AltFormat, comboBox_altFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.DistFormat, comboBox_distFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.SpeedFormat, comboBox_speedFormat.SelectedItem.ToString());

            this.Close();
        }

        private string GetValue(HashSet<SettingItem> collection, Setting setting)
        {
            return collection.FirstOrDefault(s => s.Setting == setting).Value;
        }

        private void SetIfValid(HashSet<SettingItem> collection, Setting setting, string value)
        {
            SettingItem si = collection.FirstOrDefault(s => s.Setting == setting);
            if (si.Valid(value))
            {
                si.Value = value;
            }
            else
            {
                throw new InvalidConstraintException($"Invalid value ({value}) for setting {setting.ToString()}");
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            // Just exit
            this.Close();
        }
    }
}
