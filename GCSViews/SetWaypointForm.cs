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

            if (MainV2.comPort.MAV.cs.mode == "Guided") //(MainV2.comPort.MAV.cs.mode.ToLower() == "guided")
            {
                MainV2.comPort.setGuidedModeWP(new Locationwp
                {
                    alt = _Alt,
                    lat = _Lat,
                    lng = _Lon,
                    frame = (byte)frame
                });
            }


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

        private void button2_Click(object sender, EventArgs e)
        {
            ReadCoordinate();

            //MainV2.comPort.MAV.GuidedMode.

            


            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
            {
                alt = (float)_Alt + 10,
                lat = (float)_Lat, //+ followTestCounter,          
                lng = (float)_Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            });
        }

        private void btn_Go2_Click(object sender, EventArgs e)
        {
            Locationwp gotohere = new Locationwp();

            gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
            gotohere.alt = (float)_Alt;
            gotohere.lat = (float)_Lat;
            gotohere.lng = (float)_Lon;

            MainV2.comPort.setGuidedModeWP(gotohere, true);


            //MainV2.comPort.setGuidedModeWP(
            //    new Locationwp().Set((float)_Lat, (float)_Lon, MainV2.comPort.MAV.GuidedMode.z,
            //        (ushort)MAVLink.MAV_CMD.WAYPOINT), false);

            //MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent,
            //            MAVLink.MAV_CMD.OVERRIDE_GOTO, 0, 0, 0, 0, 0, 0, takeoffAlt);

            //MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent,
            //            MAVLink.MAV_CMD.WAYPOINT, 0, 0, 0, 0, 0, 0, takeoffAlt);
        }

        //_______
        private void button3_Click(object sender, EventArgs e)
        {
            ReadCoordinate();

            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;

            MainV2.comPort.setPositionTargetGlobalInt(sysid, compid,
                true, false, false, false, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                (float)_Lat, (float)_Lon, (float)_Alt + 10, 0, 0, 0, 0, 0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReadCoordinate();

            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;

            MainV2.comPort.setPositionTargetGlobalInt(sysid, compid,
                true, true, false, false, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                (float)_Lat, (float)_Lon, (float)_Alt + 10, 0, 0, 0, 0, 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReadCoordinate();

            //MainV2.comPort.MAV.GuidedMode.

            MainV2.comPort.setGuidedModeWP(
                new Locationwp().Set((float)_Lat, (float)_Lon, MainV2.comPort.MAV.GuidedMode.z,
                    (ushort)MAVLink.MAV_CMD.WAYPOINT), false);
        }

        private void btn_DirectCommand_Click(object sender, EventArgs e)
        {
            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;
            MainV2.comPort.setMode(sysid, compid, "GUIDED");

            //setPositionTargetGlobalInt((byte)sysid, (byte)compid,
            //            true, false, false, false, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
            //            gotohere.lat, gotohere.lng, gotohere.alt, 0, 0, 0, 0, 0);

            MainV2.comPort.setPositionTargetGlobalInt(sysid, compid,
                true, false, false, false, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                (float)_Lat, (float)_Lon, (float)_Alt, 0, 0, 0, 0, 0);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;
            MainV2.comPort.setMode(sysid, compid, "GUIDED");

            //setPositionTargetGlobalInt((byte)sysid, (byte)compid,
            //            true, false, false, false, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
            //            gotohere.lat, gotohere.lng, gotohere.alt, 0, 0, 0, 0, 0);

            MainV2.comPort.setPositionTargetGlobalInt(sysid, compid,
                true, false, false, true, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                (float)_Lat, (float)_Lon, (float)_Alt, 0, 0, 0, 0, 0);
        }

        private void btn_DirectOverride_Click(object sender, EventArgs e)
        {
            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;
            MainV2.comPort.setMode(sysid, compid, "GUIDED");

            //MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent,
            //            MAVLink.MAV_CMD.OVERRIDE_GOTO, 0, 0, 0, 0, _Lat, _Lon, _Alt);

            MainV2.comPort.setPositionTargetGlobalInt(sysid, compid,
                true, false, false, false, MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                (float)_Lat, (float)_Lon, (float)_Alt, 0, 0, 0, 0, 0);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;
            MainV2.comPort.setMode(sysid, compid, "GUIDED");

            ReadCoordinate();   //ez nem volt a terepiben benne


            for (int i = 0; i< 50; ++i)
            {
                MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
                {
                    alt = (float)_Alt + 10,
                    lat = (float)_Lat, //+ followTestCounter,                    
                    lng = (float)_Lon,
                    id = (ushort)MAVLink.MAV_CMD.WAYPOINT
                });
            }
            MessageBox.Show("lat: " + _Lat + " long: " + _Lon + " alt: " + _Alt);
            System.Threading.Thread.Sleep(300);

            for (int i = 0; i < 50; ++i)
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

        private void button5_Click_1(object sender, EventArgs e)
        {
            Locationwp gotohere = new Locationwp();

            gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
            gotohere.alt = _Alt; // back to m
            gotohere.lat = _Lat;
            gotohere.lng = _Lon;
            MessageBox.Show("lat: " + _Lat + " long: " + _Lon + " alt: " + _Alt);
            try
            {
                MainV2.comPort.setGuidedModeWP(gotohere);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.Message, Strings.ERROR);
            }
        }
    }
}
