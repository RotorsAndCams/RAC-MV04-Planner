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

            radioButton_PitchPitch.Checked = true;
            radioButton_UAV.Checked = true;
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            MV04_AxisPair axisPair = radioButton_PitchPitch.Checked ? MV04_AxisPair.Pitch_Pitch : MV04_AxisPair.Throttle_Zoom;
            MV04_AxisMode axisMode = radioButton_UAV.Checked ? MV04_AxisMode.UAV : MV04_AxisMode.Cam;
            
            if (joystick.enabled)
            {
                joystick.MV04_SetAxis(axisPair, axisMode);
            }
        }
    }
}
