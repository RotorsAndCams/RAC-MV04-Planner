using MV04.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Camera
{
    public partial class CameraSettingsForm : Form
    {
        private const int CHANGE_WINDOW_WIDTH = 225;
        private const int CHANGE_WINDOW_HEIGHT = 360;
        private static CameraSettingsForm _instance;
        private bool _isFunctionsDisabled = true;
        private bool _isSettingsVisible = false;

        public static CameraSettingsForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CameraSettingsForm();
                return _instance;
            }
        }

        private CameraSettingsForm()
        {
            InitializeComponent();

            DisableControls();
            HideSettings();
            this.KeyPreview = true;

        }

        private void HideSettings()
        {
            this.uc_CameraSettings.Visible = false;
            this.Size = new System.Drawing.Size(Size.Width, Size.Height - (CHANGE_WINDOW_HEIGHT));
            _isSettingsVisible = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.E))
            {
                if (_isFunctionsDisabled)
                {
                    this.Size = new System.Drawing.Size(Size.Width + CHANGE_WINDOW_WIDTH, Size.Height);
                    this.pnl_DisabledControlsByDefault.Visible = true;
                    _isFunctionsDisabled = false;
                }
                else
                {
                    DisableControls();
                }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DisableControls()
        {
            this.Size = new System.Drawing.Size(Size.Width - CHANGE_WINDOW_WIDTH, Size.Height);
            this.pnl_DisabledControlsByDefault.Visible = false;
            _isFunctionsDisabled = true;
        }

        private void btn_DayCamera_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.SetImageSensor(false); //Set to Day camera
        }

        private void btn_NightCamera_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.SetImageSensor(true); //Set to Night camera
        }

        private void btn_NUC_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.DoNUC();
        }

        private void btn_Reconnect_Click(object sender, EventArgs e)
        {
            if (event_ReconnectRequested != null)
                event_ReconnectRequested(sender, e);
        }

        public event EventHandler event_ReconnectRequested;

        private void btn_AdvancedSettings_Click(object sender, EventArgs e)
        {
            //SettingManager.OpenDialog();

            if (_isSettingsVisible)
            {
                HideSettings();
                btn_AdvancedSettings.BackColor = Color.Black;
            }
            else
            {
                this.uc_CameraSettings.Visible = true;
                this.Size = new System.Drawing.Size(Size.Width, Size.Height + CHANGE_WINDOW_HEIGHT);
                _isSettingsVisible = true;
                btn_AdvancedSettings.BackColor = Color.DarkGreen;
            }

        }
    }
}
