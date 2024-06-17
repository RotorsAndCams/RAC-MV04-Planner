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
        private static CameraSettingsForm _instance;

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
        }
    }
}
