using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Settings
{
    public partial class uc_CameraSettings : UserControl
    {
        private HashSet<SettingItem> returnData;

        public uc_CameraSettings()
        {
            InitializeComponent();

            SetSavedSettingsValues();
        }

        private void SetSavedSettingsValues()
        {
            returnData = SettingManager.GetSettings();

            textBox_cameraIp.Text = GetValue(returnData, Setting.CameraIP);
            comboBox_cameraStreamChannel.SelectedItem = GetValue(returnData, Setting.CameraStreamChannel);
            textBox_cameraControlPort.Text = GetValue(returnData, Setting.CameraControlPort);
            radioButton_AutoConnect_Yes.Checked = bool.Parse(GetValue(returnData, Setting.AutoConnect));
            radioButton_AutoConnect_No.Checked = !radioButton_AutoConnect_Yes.Checked;
            numericUpDown_VideoSegmentLength.Value = int.Parse(GetValue(returnData, Setting.VideoSegmentLength));
            comboBox_IrColorMode.SelectedItem = GetValue(returnData, Setting.IrColorMode);
            comboBox_coordFormat.SelectedItem = GetValue(returnData, Setting.GPSType);
            comboBox_altFormat.SelectedItem = GetValue(returnData, Setting.AltFormat);
            comboBox_distFormat.SelectedItem = GetValue(returnData, Setting.DistFormat);
            comboBox_speedFormat.SelectedItem = GetValue(returnData, Setting.SpeedFormat);

            rb_AutoRecordYes.Checked = bool.Parse(GetValue(returnData, Setting.AutoRecordVideoStream));
            rb_AutoRecordNo.Checked = !rb_AutoRecordYes.Checked;

            rb_YesSY.Checked = bool.Parse(GetValue(returnData, Setting.AutoStartSingleYaw));
            rb_NoSY.Checked = !rb_YesSY.Checked;
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
                // TODO: Error message?
            }
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            SetIfValid(returnData, Setting.CameraIP, textBox_cameraIp.Text);
            SetIfValid(returnData, Setting.CameraStreamChannel, comboBox_cameraStreamChannel.SelectedItem.ToString());
            SetIfValid(returnData, Setting.CameraControlPort, textBox_cameraControlPort.Text);
            SetIfValid(returnData, Setting.AutoConnect, radioButton_AutoConnect_Yes.Checked.ToString());
            SetIfValid(returnData, Setting.VideoSegmentLength, numericUpDown_VideoSegmentLength.Value.ToString());
            SetIfValid(returnData, Setting.IrColorMode, comboBox_IrColorMode.SelectedItem.ToString());
            SetIfValid(returnData, Setting.GPSType, comboBox_coordFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.AltFormat, comboBox_altFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.DistFormat, comboBox_distFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.SpeedFormat, comboBox_speedFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.AutoRecordVideoStream, radioButton_AutoConnect_Yes.Checked.ToString());
            SetIfValid(returnData, Setting.AutoStartSingleYaw, rb_YesSY.Checked.ToString());

            SettingManager.Save(returnData);

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            SetSavedSettingsValues();
        }
    }
}
