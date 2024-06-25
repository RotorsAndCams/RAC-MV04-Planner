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
    public enum enum_MV04_CameraModes
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

            comboBox1.Items.AddRange(Enum.GetNames(typeof(enum_MV04_CameraModes)));
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((enum_MV04_CameraModes)Enum.Parse(typeof(enum_MV04_CameraModes), comboBox1.SelectedItem.ToString()))
            {
                case enum_MV04_CameraModes.Stow:
                case enum_MV04_CameraModes.Pilot:
                case enum_MV04_CameraModes.HoldCoordinate:
                case enum_MV04_CameraModes.Observation:
                case enum_MV04_CameraModes.GRR:
                case enum_MV04_CameraModes.Nadir:
                case enum_MV04_CameraModes.UnstabilizedPosition:
                    CameraHandler.Instance.SetMode((MavProto.NvSystemModes)Enum.Parse(typeof(MavProto.NvSystemModes), comboBox1.SelectedItem.ToString()));
                    break;
                case enum_MV04_CameraModes.Tracking:
                    CameraHandler.Instance.StartTracking(null); // null = track to screen center
                    break;
                case enum_MV04_CameraModes.Retract:
                    CameraHandler.Instance.Retract();
                    break;
                default: break;
            }
        }
    }
}
