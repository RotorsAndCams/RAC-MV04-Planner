﻿using log4net;
using MissionPlanner.Utilities;
using System.Globalization;
using System.Reflection;

namespace MissionPlanner.Swarm
{
    abstract class Swarm
    {
        internal static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        internal MAVState Leader = null;

        public void setLeader(MAVState lead)
        {
            Leader = lead;
        }

        public MAVState getLeader()
        {
            return Leader;
        }

        public void Arm()
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == Leader)
                        continue;

                    port.doARM(mav.sysid, mav.compid, true);
                }
            }
        }

        public void Disarm()
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == Leader)
                        continue;

                    port.doARM(mav.sysid, mav.compid, false);
                }
            }
        }

        public void Takeoff()
        {
            float takeoffAlt = float.Parse(Settings.Instance["takeoff_alt", "10"], CultureInfo.InvariantCulture);
            Settings.Instance["takeoff_alt"] = takeoffAlt.ToString(CultureInfo.InvariantCulture);

            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == Leader)
                        continue;

                    port.setMode(mav.sysid, mav.compid, "GUIDED");

                    port.doCommand(mav.sysid, mav.compid, MAVLink.MAV_CMD.TAKEOFF, 0, 0, 0, 0, 0, 0, takeoffAlt);
                }
            }
        }

        public void Land()
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    port.setMode(mav.sysid, mav.compid, "Land");
                }
            }
        }

        public void Stop()
        {
        }

        public void GuidedMode()
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == Leader)
                        continue;

                    port.setMode(mav.sysid, mav.compid, "GUIDED");
                }
            }
        }

        public void AutoMode()
        {
            foreach (var port in MainV2.Comports)
            {
                foreach (var mav in port.MAVlist)
                {
                    if (mav == Leader)
                        continue;

                    port.setMode(mav.sysid, mav.compid, "AUTO");
                }
            }
        }

        public abstract void Update();

        public abstract void SendCommand();
    }
}