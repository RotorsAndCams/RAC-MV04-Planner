﻿using MV04.Joystick;
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
    public partial class Joy_MV04_Arm : Form
    {
        public Joy_MV04_Arm(string tag)
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);
            this.BringToFront();

            this.Tag = tag;
            comboBox1.Items.AddRange(Enum.GetNames(typeof(buttonfunction_mv04_Arm_option)));
            JoyButton jb = MainV2.joystick.getButton(int.Parse(tag));
            comboBox1.SelectedIndex = (int)Math.Round(jb.p1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tag = int.Parse(this.Tag.ToString());

            JoyButton jb = MainV2.joystick.getButton(tag);
            jb.function = buttonfunction.MV04_Arm;
            jb.p1 = comboBox1.SelectedIndex;

            MainV2.joystick.setButton(tag, jb);
        }
    }
}
