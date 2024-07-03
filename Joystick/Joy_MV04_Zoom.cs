using MV04.GCS;
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
    public partial class Joy_MV04_Zoom : Form
    {
        public Joy_MV04_Zoom(string tag)
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);
            this.BringToFront();

            this.Tag = tag;
<<<<<<<< HEAD:Joystick/Joy_MV04_Zoom.cs
            comboBox1.Items.AddRange(Enum.GetNames(typeof(buttonfunction_mv04_Zoom_option)));
========
            comboBox1.Items.AddRange(Enum.GetNames(typeof(buttonfunction_mv04_ImageSensor_option)));
>>>>>>>> main:Joystick/Joy_MV04_CameraMode.cs
            JoyButton jb = MainV2.joystick.getButton(int.Parse(tag));
            comboBox1.SelectedIndex = (int)Math.Round(jb.p1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tag = int.Parse(this.Tag.ToString());
            JoyButton jb = MainV2.joystick.getButton(tag);

<<<<<<<< HEAD:Joystick/Joy_MV04_Zoom.cs
            jb.function = buttonfunction.MV04_Zoom;
========
            jb.function = buttonfunction.MV04_ImageSensor;
>>>>>>>> main:Joystick/Joy_MV04_CameraMode.cs
            jb.p1 = comboBox1.SelectedIndex;

            MainV2.joystick.setButton(tag, jb);
        }
    }
}
