using MV04.State;
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
    public partial class GCSModeTesterForm : Form
    {
        public GCSModeTesterForm()
        {
            InitializeComponent();

            cb_SelectMode.Items.AddRange(Enum.GetNames(typeof(MV04_State)));
            cb_SelectMode.SelectedIndex = 0;

        }

        private void btn_SetMode_Click(object sender, EventArgs e)
        {
            StateHandler.CurrentSate = (MV04_State)Enum.Parse(typeof(MV04_State), cb_SelectMode.SelectedItem.ToString());
        }
    }
}
