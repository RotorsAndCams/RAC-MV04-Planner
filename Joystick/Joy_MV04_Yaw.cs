using MV04.Joystick;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.Joystick
{
    public partial class Joy_MV04_Yaw : Form
    {
        public Joy_MV04_Yaw(string tag)
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);
            this.BringToFront();

            this.Tag = tag;
            JoyButton jb = MainV2.joystick.getButton(int.Parse(tag));

            comboBox1.Items.AddRange(Enum.GetNames(typeof(buttonfunction_mv04_Yaw_option)));
            comboBox1.SelectedIndex = (int)Math.Round(jb.p1);
            numericUpDown1.Value = (decimal)jb.p2;
            if (numericUpDown1.Value <= 0) numericUpDown1.Value = (decimal)0.5; // default value
            if (numericUpDown1.Value > 1) numericUpDown1.Value = 1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tag = int.Parse(this.Tag.ToString());
            JoyButton jb = MainV2.joystick.getButton(tag);

            jb.function = buttonfunction.MV04_Yaw;
            jb.p1 = comboBox1.SelectedIndex;

            MainV2.joystick.setButton(tag, jb);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int tag = int.Parse(this.Tag.ToString());
            JoyButton jb = MainV2.joystick.getButton(tag);

            jb.function = buttonfunction.MV04_Yaw;
            jb.p2 = (float)numericUpDown1.Value;
            if (jb.p2 < 0) jb.p2 = 0;
            if (jb.p2 > 1) jb.p2 = 1;

            MainV2.joystick.setButton(tag, jb);
        }
    }
}
