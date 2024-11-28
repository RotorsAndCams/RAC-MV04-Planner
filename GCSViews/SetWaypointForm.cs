using MissionPlanner.ArduPilot;
using MissionPlanner.Utilities;
using MV04.SingleYaw;
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

        private void btn_Solution1_Click(object sender, EventArgs e)
        {
            SingleYawHandler.StopSingleYaw();
            ReadCoordinate();

            MainV2.comPort.setGuidedModeWP(new Locationwp
            {
                alt = _Alt,//MainV2.comPort.MAV.GuidedMode.z,
                lat = _Lat,//MainV2.comPort.MAV.GuidedMode.x / 1e7,
                lng = _Lon//MainV2.comPort.MAV.GuidedMode.y / 1e7
            });
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }

        private void btn_Solution2_Click(object sender, EventArgs e)
        {
            SingleYawHandler.StopSingleYaw();
            ReadCoordinate();

            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
            {
                alt = _Alt,
                lat = _Lat,
                lng = _Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            });
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
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

            MainV2.comPort.setGuidedModeWP((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, new Locationwp()
            {
                alt = (float)_Alt + 10,
                lat = (float)_Lat, //+ followTestCounter,          
                lng = (float)_Lon,
                id = (ushort)MAVLink.MAV_CMD.WAYPOINT
            });

            MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.MISSION_START, 0,
                        0, 0, 0, 0, 0, 0);

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

        

        private void btn_MavWPSet_Click(object sender, EventArgs e)
        {

            ReadCoordinate();

            byte sysid = (byte)MainV2.comPort.sysidcurrent;
            byte compid = (byte)MainV2.comPort.compidcurrent;

            Locationwp gotohere = new Locationwp();

            gotohere.id = (ushort)MAVLink.MAV_CMD.WAYPOINT;
            gotohere.alt = (float)_Alt;
            gotohere.lat = (float)_Lat;
            gotohere.lng = (float)_Lon;

            MainV2.comPort.setGuidedModeWP(gotohere, true);

            var tsk = MainV2.comPort.setWPCurrentAsync(sysid, compid, (ushort)MAVLink.MAV_CMD.WAYPOINT).GetAwaiter();
        }

        private void btn_Timing_Click(object sender, EventArgs e)
        {
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");

            MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, true, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }

        private void btn_NotWP_Click(object sender, EventArgs e)
        {
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");
            Thread.Sleep(500);
            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, true, false, false,
                MAVLink.MAV_FRAME.GLOBAL_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }

        private void btn_MS_Click(object sender, EventArgs e)
        {
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");

            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, false, false, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }

            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }

        

        private void btn_ModIData_Click(object sender, EventArgs e)
        {
            ReadCoordinate();
            MainV2.comPort.MAV.GuidedMode.x = (int)(_Lat * 1e7);
            MainV2.comPort.MAV.GuidedMode.y = (int)(_Lon * 1e7);
            MainV2.comPort.MAV.GuidedMode.x = _Alt;

            MainV2.comPort.setGuidedModeWP(new Locationwp
            {
                alt = MainV2.comPort.MAV.GuidedMode.z,
                lat = MainV2.comPort.MAV.GuidedMode.x / 1e7,
                lng = MainV2.comPort.MAV.GuidedMode.y / 1e7
            });
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");

            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, true, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }

            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }
        ////
        ///
        private void btn_SetWPCurrentAsync_Click(object sender, EventArgs e)
        {
            //duplaklikkre ment de írt távolságot
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");

            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, false, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 1, 1);
            }
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }
        private void button5_Click_1(object sender, EventArgs e) //ment de nem irt távolságot
        {
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");

            for (int i = 0; i <= 200; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, true, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");
            Thread.Sleep(500);

            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, true, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //ez a jo
            ReadCoordinate();
            SingleYawHandler.StopSingleYaw();

            MainV2.comPort.setMode(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, "GUIDED");
            Thread.Sleep(500);
            for (int i = 0; i <= 100; i++)
            {
                MainV2.comPort.setPositionTargetGlobalInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, true, false, false, false,
                MAVLink.MAV_FRAME.GLOBAL_RELATIVE_ALT_INT,
                _Lat, _Lon, _Alt, Vector3.Zero.x, Vector3.Zero.y, Vector3.Zero.z, 0, 0);
            }
            SingleYawHandler.StartSingleYaw(MainV2.comPort);
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
