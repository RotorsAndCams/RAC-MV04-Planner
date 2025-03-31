using MV04.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class uc_CameraSettings : UserControl
    {
        private HashSet<SettingItem> returnData;

        public uc_CameraSettings()
        {
            InitializeComponent();

            SetDevModeGUI();

            SetSavedSettingsValues();
        }

        private void SetDevModeGUI()
        {
            if (MainV2.instance.devmode)
            {
                this.lb_CamIP.Visible = true;
                this.tb_CamIP.Visible = true;

                this.lb_CamPort.Visible = true;
                this.tb_CamPort.Visible = true;

                this.lb_CamStream.Visible = true;
                this.tb_StreamUrl.Visible = true;

                this.lb_AutoConnectCam.Visible = true;
                this.tlp_AutoConnCam.Visible = true;

                this.lb_AutoStartSingleYaw.Visible = true;
                this.tlp_SYAutoStart.Visible = true;

                this.lb_AutoStartCameraStream.Visible = true;
                this.tlp_AutoStartCameraStream.Visible = true;
            }
            else
            {
                numericUpDown_VideoSegmentLength.Height = numericUpDown_VideoSegmentLength.Height * 2;
                numericUpDown_VideoSegmentLength.Font = new Font(numericUpDown_VideoSegmentLength.Font.FontFamily, numericUpDown_VideoSegmentLength.Font.Size *2);

                comboBox_IrColorMode.Height = comboBox_IrColorMode.Height * 2;
                comboBox_IrColorMode.Font = new Font(comboBox_IrColorMode.Font.FontFamily, comboBox_IrColorMode.Font.Size + 4);

                comboBox_coordFormat.Height = comboBox_coordFormat.Height * 2;
                comboBox_coordFormat.Font = new Font(comboBox_coordFormat.Font.FontFamily, comboBox_coordFormat.Font.Size + 4);

                comboBox_altFormat.Height = comboBox_altFormat.Height * 2;
                comboBox_altFormat.Font = new Font(comboBox_altFormat.Font.FontFamily, comboBox_altFormat.Font.Size + 4);

                comboBox_distFormat.Height = comboBox_distFormat.Height * 2;
                comboBox_distFormat.Font = new Font(comboBox_distFormat.Font.FontFamily, comboBox_distFormat.Font.Size + 4);

                comboBox_speedFormat.Height = comboBox_speedFormat.Height * 2;
                comboBox_speedFormat.Font = new Font(comboBox_speedFormat.Font.FontFamily, comboBox_speedFormat.Font.Size + 4);

                rb_AutoRecordYes.Height = rb_AutoRecordYes.Height * 2;
                rb_AutoRecordYes.Font = new Font(rb_AutoRecordYes.Font.FontFamily, rb_AutoRecordYes.Font.Size + 4);

                rb_AutoRecordNo.Height = rb_AutoRecordNo.Height * 2;
                rb_AutoRecordNo.Font = new Font(rb_AutoRecordNo.Font.FontFamily, rb_AutoRecordNo.Font.Size + 4);


                tlp_Base.RowStyles[0].Height = 100;
                tlp_Base.RowStyles[0].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[1].Height = 100;
                tlp_Base.RowStyles[1].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[2].Height = 100;
                tlp_Base.RowStyles[2].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[3].Height = 100;
                tlp_Base.RowStyles[3].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[4].Height = 100;
                tlp_Base.RowStyles[4].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[5].Height = 100;
                tlp_Base.RowStyles[5].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[6].Height = 100;
                tlp_Base.RowStyles[6].SizeType = SizeType.Percent;

                //7 8 9 10 11 12 legyen 0 és 13 max

                tlp_Base.RowStyles[7].Height = 0;
                tlp_Base.RowStyles[7].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[8].Height = 0;
                tlp_Base.RowStyles[8].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[9].Height = 0;
                tlp_Base.RowStyles[9].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[10].Height = 0;
                tlp_Base.RowStyles[10].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[11].Height = 0;
                tlp_Base.RowStyles[11].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[12].Height = 0;
                tlp_Base.RowStyles[12].SizeType = SizeType.Percent;

                tlp_Base.RowStyles[13].Height = 100;
                tlp_Base.RowStyles[13].SizeType = SizeType.Percent;
            }
        }

        private void SetSavedSettingsValues()
        {
            returnData = SettingManager.GetSettings();

            tb_CamIP.Text = GetValue(returnData, Setting.CameraIP);
            tb_StreamUrl.Text = GetValue(returnData, Setting.CameraStreamUrl);
            tb_CamPort.Text = GetValue(returnData, Setting.CameraControlPort);
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

            rb_AutoStartCameraStream_Yes.Checked = bool.Parse(GetValue(returnData, Setting.AutoStartCameraStream));
            rb_AutoStartCameraStream_No.Checked = !rb_AutoStartCameraStream_Yes.Checked;
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

        private void button_Save_Click(object sender, EventArgs e)
        {
            SetIfValid(returnData, Setting.CameraIP, tb_CamIP.Text);
            SetIfValid(returnData, Setting.CameraStreamUrl, tb_StreamUrl.Text);
            SetIfValid(returnData, Setting.CameraControlPort, tb_CamPort.Text);
            SetIfValid(returnData, Setting.AutoConnect, radioButton_AutoConnect_Yes.Checked.ToString());
            SetIfValid(returnData, Setting.VideoSegmentLength, numericUpDown_VideoSegmentLength.Value.ToString());
            SetIfValid(returnData, Setting.IrColorMode, comboBox_IrColorMode.SelectedItem.ToString());
            SetIfValid(returnData, Setting.GPSType, comboBox_coordFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.AltFormat, comboBox_altFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.DistFormat, comboBox_distFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.SpeedFormat, comboBox_speedFormat.SelectedItem.ToString());
            SetIfValid(returnData, Setting.AutoRecordVideoStream, rb_AutoRecordYes.Checked.ToString());
            SetIfValid(returnData, Setting.AutoStartSingleYaw, rb_YesSY.Checked.ToString());
            SetIfValid(returnData, Setting.AutoStartCameraStream, rb_AutoStartCameraStream_Yes.Checked.ToString());

            SettingManager.Save();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            SetSavedSettingsValues();
        }
    }
}
