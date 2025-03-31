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
    public partial class StreamUrlForm : Form
    {
        public StreamUrlForm(string streamUrl)
        {
            InitializeComponent();

            this.tb_Url.Text = streamUrl;
        }

        private void btn_SetUrl_Click(object sender, EventArgs e)
        {
            CameraView.instance.SetNewURL(tb_Url.Text);
        }
    }
}
