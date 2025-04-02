using MissionPlanner.ArduPilot;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;
using static MAVLink;
using static MissionPlanner.Utilities.LTM;

namespace MissionPlanner.GCSViews
{
    public partial class SetWaypointForm : Form
    {
        float _Lat;
        float _Lon;
        int _Alt;

        public SetWaypointForm()
        {
            InitializeComponent();

            _Lat = (float)MainV2.comPort.MAV.cs.lat;
            _Lon = (float)MainV2.comPort.MAV.cs.lng;
            _Alt = (int)MainV2.comPort.MAV.cs.alt;

            this.rtb_Latitude.Text = _Lat.ToString();
            this.rtb_Longitude.Text = _Lon.ToString();
            this.rtb_Altitude.Text = _Alt.ToString();
        }

        private void ReadCoordinate()
        {
            try
            {
                _Lat = float.Parse(rtb_Latitude.Text);
                _Lon = float.Parse(rtb_Longitude.Text);
                _Alt = Convert.ToInt32(rtb_Altitude.Text);
            }
            catch { MessageBox.Show("Not valid coordinate"); }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //worked

            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
            {   
                alt = (float)_Alt + 10,
                lat = (float)_Lat, //+ followTestCounter,                    
                lng = (float)_Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            });
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;
            MainV2.comPort.setMode(sysid, compid, "GUIDED");

            Locationwp gotohere = new Locationwp();

            gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
            gotohere.alt = _Alt; // back to m
            gotohere.lat = _Lat;
            gotohere.lng = _Lon;

            ReadCoordinate();   //ez nem volt a terepiben benne
            MainV2.comPort.setWP(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, gotohere, (ushort)MAVLink.MAV_CMD.WAYPOINT, MAVLink.MAV_FRAME.GLOBAL_INT);
            //MainV2.comPort.setWP(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, gotohere, (ushort)MAVLink.MAV_CMD.WAYPOINT, MAVLink.MAV_FRAME.GLOBAL_INT);

            //MainV2.comPort.setWPACK();
            MainV2.comPort.setWPCurrent(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, 0);

            for (int i = 0; i< 200; ++i)
            {
                MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, gotohere);
            }
            
        }

        
        private void button8_Click(object sender, EventArgs e)
        {
            //ez a jo
            ReadCoordinate();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");
            Thread.Sleep(500);
            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, false, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            var custom_mode = (MainV2.comPort.MAV.cs.sensors_enabled.motor_control && MainV2.comPort.MAV.cs.sensors_enabled.seen) ? 1u : 0u;
            var mode = new MAVLink.mavlink_set_mode_t() { custom_mode = custom_mode, target_system = (byte)MainV2.comPort.sysidcurrent };

            MainV2.comPort.translateMode((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, "GUIDED", ref mode);

            MainV2.comPort.setMode((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent,
                mode, MAV_MODE_FLAG.GUIDED_ENABLED);

        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            ReadCoordinate();
            Locationwp gotohere = new Locationwp() {
                alt = _Alt,
                lat = _Lat,
                lng = _Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            };

            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, gotohere, false);
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            ReadCoordinate();

            Locationwp gotohere = new Locationwp()
            {
                alt = _Alt,
                lat = _Lat,
                lng = _Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            };
            for (int i = 0; i <= 200; i++)
            {
                MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, gotohere, false);
            }
        }
    }
}
