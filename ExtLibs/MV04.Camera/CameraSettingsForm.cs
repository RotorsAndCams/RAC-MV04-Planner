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
        private const int CHANGEWINDOWHEIGHT = 272;
        private static CameraSettingsForm _instance;
        private bool _reducedSize = true;

        public static CameraSettingsForm Instance
        {
            get
            {
                if(_instance == null || _instance.IsDisposed)
                    _instance = new CameraSettingsForm();
                return _instance;
            }
        }

        private CameraSettingsForm()
        {
            InitializeComponent();

            DisableControls();
            this.KeyPreview = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.E))
            {
                if (_reducedSize)
                {
                    this.Size = new System.Drawing.Size(Size.Width, Size.Height + CHANGEWINDOWHEIGHT);
                    this.pnl_DisabledControlsByDefault.Visible = true;
                    _reducedSize = false;
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
            this.Size = new System.Drawing.Size(Size.Width, Size.Height - CHANGEWINDOWHEIGHT);
            this.pnl_DisabledControlsByDefault.Visible = false;
            _reducedSize = true;
        }

        private void btn_DayCamera_Click(object sender, EventArgs e)
        {
            CameraHandler.SetImageSensorAsync(false); //Set to Day camera
        }

        private void btn_NightCamera_Click(object sender, EventArgs e)
        {
            CameraHandler.SetImageSensorAsync(true); //Set to Night camera
        }

        private void btn_NUC_Click(object sender, EventArgs e)
        {
            CameraHandler.DoNUCAsync();
        }

        private void btn_Reconnect_Click(object sender, EventArgs e)
        {
            if (event_ReconnectRequested != null)
                event_ReconnectRequested(sender, e);
        }

        public event EventHandler event_ReconnectRequested;
    }
}
