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
    public partial class WindInputForm : Form
    {
        public WindInputFormData ReturnData { get; private set; }

        public WindInputForm(WindInputFormData formData)
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);

            ReturnData = new WindInputFormData();
            if (formData.WindDir >= (double)numericUpDown_dir.Minimum
                && formData.WindDir <= (double)numericUpDown_dir.Maximum)
            {
                numericUpDown_dir.Value = (decimal)formData.WindDir;
                ReturnData.WindDir = formData.WindDir;
            }
            if (formData.WindSpeed >= (double)numericUpDown_speed.Minimum
                && formData.WindSpeed <= (double)numericUpDown_speed.Maximum)
            {
                numericUpDown_speed.Value = (decimal)formData.WindSpeed;
                ReturnData.WindSpeed = formData.WindSpeed;
            }

            label_error.Text = "";

            this.BringToFront();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            ReturnData.WindDir = (double)numericUpDown_dir.Value;
            ReturnData.WindSpeed = (double)numericUpDown_speed.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.Cancel;
            this.Close();
        }
    }

    public class WindInputFormData
    {
        public double WindDir { get; set; }
        public double WindSpeed { get; set; }
    }
}
