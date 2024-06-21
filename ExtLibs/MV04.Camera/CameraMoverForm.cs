using MissionPlanner.Comms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MV04.Camera
{
    public partial class CameraMoverForm : Form
    {
        System.Windows.Forms.Timer GimbalTimer;

        (int roll, int pitch, int zoom) Inputs;
        (float roll, float pitch, int zoom) LastRates;

        public CameraMoverForm(int gimbalUpdateMs = 100)
        {
            InitializeComponent();
            BringToFront();

            Inputs = (0, 0, 0);
            LastRates = (0, 0, 0);

            GimbalTimer = new System.Windows.Forms.Timer();
            GimbalTimer.Interval = gimbalUpdateMs;
            GimbalTimer.Tick += GimbalTimer_Tick;
            GimbalTimer.Start();
        }

        private void CameraMoverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void GimbalTimer_Tick(object sender, EventArgs e)
        {
            // Calculate new rates
            (float roll, float pitch, int zoom) NewRates =
                (
                    Inputs.roll == 0 ? 0 : LastRates.roll + (Inputs.roll * 0.05f),
                    Inputs.pitch == 0 ? 0 : LastRates.pitch + (Inputs.pitch * 0.05f),
                    Inputs.zoom == 0 ? 0 : Inputs.zoom == 1 ? 1 : 2
                );

            // Clamp values
            if (NewRates.roll > 1) NewRates.roll = 1;
            if (NewRates.roll < -1) NewRates.roll = -1;
            if (NewRates.pitch > 1) NewRates.pitch = 1;
            if (NewRates.pitch < -1) NewRates.pitch = -1;

            // Send if new
            if (NewRates.roll != LastRates.roll || NewRates.pitch != LastRates.pitch || NewRates.zoom != LastRates.zoom)
            {
                CameraHandler.Instance.MoveCamera(NewRates.roll, NewRates.pitch, NewRates.zoom, 0);

                label1.Text = $"{Math.Round(NewRates.roll, 2)},{Math.Round(NewRates.pitch, 2)},{NewRates.zoom}";

                LastRates.roll = NewRates.roll;
                LastRates.pitch = NewRates.pitch;
                LastRates.zoom = NewRates.zoom;
            }
        }

        private void button_ZoomIn_MouseDown(object sender, MouseEventArgs e) => Inputs.zoom = 1;

        private void button_ZoomIn_MouseUp(object sender, MouseEventArgs e) => Inputs.zoom = 0;

        private void button_ZoomOut_MouseDown(object sender, MouseEventArgs e) => Inputs.zoom = -1; // 2

        private void button_Up_MouseDown(object sender, MouseEventArgs e) => Inputs.pitch = 1;

        private void button_Up_MouseUp(object sender, MouseEventArgs e) => Inputs.pitch = 0;

        private void button_Down_MouseDown(object sender, MouseEventArgs e) => Inputs.pitch = -1;

        private void button_Right_MouseDown(object sender, MouseEventArgs e) => Inputs.roll = -1;

        private void button_Right_MouseUp(object sender, MouseEventArgs e) => Inputs.roll = 0;

        private void button_Left_MouseDown(object sender, MouseEventArgs e) => Inputs.roll = 1;

        private void button_Center_Click(object sender, EventArgs e)
        {
            Inputs.roll = 0;
            Inputs.pitch = 0;
            Inputs.zoom = 0;
        }
    }
}
