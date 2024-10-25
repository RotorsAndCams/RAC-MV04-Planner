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
        public CameraMoverForm()
        {
            InitializeComponent();
            BringToFront();
        }

        private void CameraMoverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button_ZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetZoom(ZoomState.In);
        }

        private void button_ZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetZoom(ZoomState.Stop);
        }

        private void button_ZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetZoom(ZoomState.Out);
        }

        private void button_Up_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetCameraPitch(PitchDirection.Up);
        }

        private void button_Up_MouseUp(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetCameraPitch(PitchDirection.Stop);
        }

        private void button_Down_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetCameraPitch(PitchDirection.Down);
        }

        private void button_Right_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetCameraYaw(YawDirection.Right);
        }

        private void button_Right_MouseUp(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetCameraYaw(YawDirection.Stop);
        }

        private void button_Left_MouseDown(object sender, MouseEventArgs e)
        {
            CameraHandler.Instance.SetCameraYaw(YawDirection.Left);
        }

        private void button_Center_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.CenterCamera();
        }
    }
}
