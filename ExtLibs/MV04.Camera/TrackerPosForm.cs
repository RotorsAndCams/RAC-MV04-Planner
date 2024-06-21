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
    public partial class TrackerPosForm : Form
    {
        public TrackerPosForm()
        {
            InitializeComponent();
            BringToFront();
        }

        private void button_StartTracking_Click(object sender, EventArgs e) => CameraHandler.Instance.StartTracking(new Point((int)numericUpDown_X.Value, (int)numericUpDown_Y.Value));

        private void button_StopTracking_Click(object sender, EventArgs e) => CameraHandler.Instance.StopTracking(true);
    }
}
