using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Camera
{
    public partial class CameraSettingsForm : Form
    {
        private const int CHANGEWINDOWHEIGHT = 255;
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

        public CameraSettingsForm()
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
    }
}
