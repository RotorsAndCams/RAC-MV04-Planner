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

            comboBox1.Items.AddRange(Enum.GetNames(typeof(MavProto.NvSystemModes)));
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch ((MavProto.NvSystemModes)Enum.Parse(typeof(MavProto.NvSystemModes), comboBox1.SelectedItem.ToString()))
            //{
            //    case MavProto.NvSystemModes.Stow:
            //    case MavProto.NvSystemModes.Pilot:
            //    case MavProto.NvSystemModes.HoldCoordinate:
            //    case MavProto.NvSystemModes.Observation:
            //    case MavProto.NvSystemModes.GRR:
            //    case MavProto.NvSystemModes.Nadir:
            //    case MavProto.NvSystemModes.UnstabilizedPosition:
            //        CameraHandler.Instance.SetMode((MavProto.NvSystemModes)Enum.Parse(typeof(MavProto.NvSystemModes), comboBox1.SelectedItem.ToString()));
            //        break;
            //    case MavProto.NvSystemModes.Tracking:
            //        CameraHandler.Instance.StartTracking(null); // null = track to screen center
            //        break;
            //    //case MavProto.NvSystemModes.:
            //    //    CameraHandler.Instance.Retract();
            //        break;
            //    default: break;
            //}
            CameraHandler.Instance.SetMode((MavProto.NvSystemModes)Enum.Parse(typeof(MavProto.NvSystemModes), comboBox1.SelectedItem.ToString()));

            if(((MavProto.NvSystemModes)Enum.Parse(typeof(MavProto.NvSystemModes), comboBox1.SelectedItem.ToString()) == MavProto.NvSystemModes.Tracking))
                CameraHandler.Instance.StartTracking(null);
        }
    }
}
