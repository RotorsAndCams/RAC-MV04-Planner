using MissionPlanner.ArduPilot;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class SetWaypointForm : Form
    {
        double _Lat;
        double _Lon;
        int _Alt;

        public SetWaypointForm()
        {
            InitializeComponent();

            _Lat = MainV2.comPort.MAV.cs.lat;
            _Lon = MainV2.comPort.MAV.cs.lng;
            _Alt = (int)MainV2.comPort.MAV.cs.alt;

            this.rtb_Latitude.Text = _Lat.ToString();
            this.rtb_Longitude.Text = _Lon.ToString();
            this.rtb_Altitude.Text = _Alt.ToString();
        }

        private void ReadCoordinate()
        {
            try
            {
                _Lat = Convert.ToDouble(rtb_Latitude.Text);
                _Lon = Convert.ToDouble(rtb_Longitude.Text);
                _Alt = Convert.ToInt32(rtb_Altitude.Text);
            }
            catch { MessageBox.Show("Not valid coordinate"); }
        }

        private void btn_Solution1_Click(object sender, EventArgs e)
        {
            ReadCoordinate();

            MainV2.comPort.setGuidedModeWP(new Locationwp
            {
                alt = _Alt,//MainV2.comPort.MAV.GuidedMode.z,
                lat = _Lat,//MainV2.comPort.MAV.GuidedMode.x / 1e7,
                lng = _Lon//MainV2.comPort.MAV.GuidedMode.y / 1e7
            });
        }

        private void btn_Solution2_Click(object sender, EventArgs e)
        {
            ReadCoordinate();

            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
            {
                alt = _Alt,
                lat = _Lat,
                lng = _Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            });
        }

        private void btn_Solution3_Click(object sender, EventArgs e)
        {
            ReadCoordinate();


            string alt = "100";
            MAVLink.MAV_FRAME frame = MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT;

            if (MainV2.comPort.MAV.cs.firmware == Firmwares.ArduCopter2)
            {
                alt = (10 * CurrentState.multiplieralt).ToString("0");
            }
            else
            {
                alt = (100 * CurrentState.multiplieralt).ToString("0");
            }

            if (Settings.Instance.ContainsKey("guided_alt"))
                alt = Settings.Instance["guided_alt"];
            if (Settings.Instance.ContainsKey("guided_alt_frame"))
                frame = (MAVLink.MAV_FRAME)byte.Parse(Settings.Instance["guided_alt_frame"]);


            Settings.Instance["guided_alt"] = alt;
            Settings.Instance["guided_alt_frame"] = ((byte)frame).ToString();

            int intalt = (int)(100 * CurrentState.multiplieralt);
            if (!int.TryParse(alt, out intalt))
            {
                CustomMessageBox.Show("Bad Alt");
                return;
            }

            MainV2.comPort.MAV.GuidedMode.z = intalt / CurrentState.multiplieralt;
            MainV2.comPort.MAV.GuidedMode.frame = (byte)frame;

            if (MainV2.comPort.MAV.cs.mode == "Guided")
            {
                MainV2.comPort.setGuidedModeWP(new Locationwp
                {
                    alt = _Alt,
                    lat = _Lat / 1e7,
                    lng = _Lon / 1e7,
                    frame = (byte)frame
                });
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {


            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
            {   
                alt = (float)_Alt + 10,
                lat = (float)_Lat, //+ followTestCounter,                    
                lng = (float)_Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            });
        }
    }
}
