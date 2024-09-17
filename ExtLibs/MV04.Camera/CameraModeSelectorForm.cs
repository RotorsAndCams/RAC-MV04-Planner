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
    /// <summary>
    /// Contains only the used camera modes from MavProto.NvSystemModes
    /// </summary>
    public enum MV04_CameraModes
    {
        Stow = MavProto.NvSystemModes.Stow,
        Pilot = MavProto.NvSystemModes.Pilot,
        HoldCoordinate = MavProto.NvSystemModes.HoldCoordinate,
        Observation = MavProto.NvSystemModes.Observation,
        //LocalPosition = MavProto.NvSystemModes.LocalPosition,
        //GlobalPosition = MavProto.NvSystemModes.GlobalPosition,
        GRR = MavProto.NvSystemModes.GRR,
        //EPR = MavProto.NvSystemModes.EPR,
        Nadir = MavProto.NvSystemModes.Nadir,
        //Nadir_Scan = MavProto.NvSystemModes.Nadir_Scan,
        //TwoDScan = MavProto.NvSystemModes.TwoDScan,
        //PTC = MavProto.NvSystemModes.PTC,
        UnstabilizedPosition = MavProto.NvSystemModes.UnstabilizedPosition,
        Tracking = MavProto.NvSystemModes.Tracking,
        Retract
    }

    public partial class CameraModeSelectorForm : Form
    {
        public CameraModeSelectorForm()
        {
            InitializeComponent();
            BringToFront();

            comboBox1.Items.AddRange(Enum.GetNames(typeof(MV04_CameraModes)));
            comboBox1.SelectedIndex = 0;
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            MV04_CameraModes selected = (MV04_CameraModes)Enum.Parse(typeof(MV04_CameraModes), comboBox1.SelectedItem.ToString());
            switch (selected)
            {
                case MV04_CameraModes.Stow:
                case MV04_CameraModes.Pilot:
                case MV04_CameraModes.HoldCoordinate:
                case MV04_CameraModes.Observation:
                case MV04_CameraModes.GRR:
                case MV04_CameraModes.Nadir:
                case MV04_CameraModes.UnstabilizedPosition:
                    CameraHandler.Instance.SetMode((MavProto.NvSystemModes)selected);
                    break;
                case MV04_CameraModes.Tracking:
                    CameraHandler.Instance.StartTracking(null); // null = track to screen center
                    break;
                case MV04_CameraModes.Retract:
                    CameraHandler.Instance.Retract();
                    break;
                default: break;
            }
        }
    }
}
