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
    public partial class CameraTesterForm : Form
    {
        public CameraTesterForm()
        {
            InitializeComponent();
        }

        private void btn_SetUrl_Click(object sender, EventArgs e)
        {
            string url = this.tb_Url.Text;

            CameraView.instance.SetNewURL(url);

            CustomMessageBox.Show("url is set to: " + url);
        }
    }
}
