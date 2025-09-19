using MissionPlanner.Joystick;
using MV04.Joystick;
using System;
using System.Windows.Forms;

namespace MV04.TestForms
{
    public partial class JoystickAxisSwitcherForm : Form
    {
        private JoystickBase joystick;

        public JoystickAxisSwitcherForm(JoystickBase joystick)
        {
            InitializeComponent();
            this.joystick = joystick;

            comboBox_Modes.DataSource = Enum.GetNames(typeof(MV04_JoyMode));
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            MV04_JoyMode mode = (MV04_JoyMode)comboBox_Modes.SelectedIndex;

            if (joystick.enabled)
            {
                joystick.MV04_SetRCChannels(mode);
            }
        }
    }
}
