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
    public partial class CameraModeSelectorForm : Form
    {
        enum Modes
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

        public CameraModeSelectorForm()
        {
            InitializeComponent();
            BringToFront();

            comboBox1.Items.AddRange(Enum.GetNames(typeof(Modes)));
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((Modes)Enum.Parse(typeof(Modes), comboBox1.SelectedItem.ToString()))
            {
                case Modes.Stow:
                case Modes.Pilot:
                case Modes.HoldCoordinate:
                case Modes.Observation:
                case Modes.GRR:
                case Modes.Nadir:
                case Modes.UnstabilizedPosition:
                    CameraHandler.Instance.SetMode((MavProto.NvSystemModes)Enum.Parse(typeof(MavProto.NvSystemModes), comboBox1.SelectedItem.ToString()));
                    break;
                case Modes.Tracking:
                    CameraHandler.Instance.StartTracking(null); // null = track to screen center
                    break;
                case Modes.Retract:
                    CameraHandler.Instance.Retract();
                    break;
                default: break;
            }
        }
    }
}
