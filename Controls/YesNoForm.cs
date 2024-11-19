using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class YesNoForm : Form
    {
        public YesNoForm(string text, string caption)
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);

            this.Text = caption;
            label_Text.Text = text;
        }

        private void button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
