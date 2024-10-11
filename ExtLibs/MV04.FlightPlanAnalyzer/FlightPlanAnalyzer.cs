using MissionPlanner.Utilities;
using MissionPlanner.Utilities.nfz;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using static MAVLink;

namespace MV04.FlightPlanAnalyzer
{
    public static class FlightPlanAnalyzer
    {
        #region Fields
        private static readonly double H_TO_S = 3600.0;

        private static readonly double S_TO_H = 1.0 / H_TO_S;

        private static (double AirSpeed, double Consumption)[] AirSpeedConsumptionTable =
        {
            // TODO: Populate AirSpeedConsumptionTable with measurements
            (10, 5),
            (20, 12)
        };
        #endregion

        #region Methods
        private static double AirSpeedToAh(double airSpeed)
        {
            double slope = 0;
            for (int i = 0; i < AirSpeedConsumptionTable.Length; i++)
            {
                slope += AirSpeedConsumptionTable[i].Consumption / AirSpeedConsumptionTable[i].AirSpeed;
            }
            slope /= AirSpeedConsumptionTable.Length;

            return airSpeed * slope;
        }

        private static double BatteryVoltsToAh(double currentVolts, double maxVolts, double minVolts, int fullAmpHours)
        {
            return fullAmpHours / (maxVolts - minVolts) * (currentVolts - minVolts);
            // TODO: Replace BatteryVoltsToAh with more precise (non-linear) function
        }

        private static double PointsToAh(PointLatLngAlt pos1, PointLatLngAlt pos2, double travelSpeed, double travelConsumption, double climbSpeed, double climbConsumption, double descSpeed, double descConsumption)
        {
            // Horizontal component
            double horizontalDist = pos1.GetDistance(pos2);                             // m
            double horizontalTime = horizontalDist / travelSpeed;                       // s
            double horizontalConsumption = horizontalTime * S_TO_H * travelConsumption; // Ah
            // TODO: Include the wind in the horizontal calculation

            // Vertical component
            double verticalDist = Math.Abs(pos1.Alt - pos2.Alt),    // m
                   verticalTime,                                    // s
                   verticalConsumption;                             // A
            if (pos1.Alt < pos2.Alt) // Climbing
            {
                verticalTime = verticalDist / climbSpeed;                       // s
                verticalConsumption = verticalTime * S_TO_H * climbConsumption; // Ah
            }
            else // Descending
            {
                verticalTime = verticalDist / descSpeed;                        // s
                verticalConsumption = verticalTime * S_TO_H * descConsumption;  // Ah
            }

            return horizontalConsumption + verticalConsumption;
        }

        private static double FligtplanToAh(ConcurrentDictionary<int, mavlink_mission_item_int_t> flightPlan, int nextWP, PointLatLngAlt uavLocation, PointLatLngAlt homeLocation, double travelSpeed, double travelConsumption, double climbSpeed, double climbConsumption, double descSpeed, double descConsumption)
        {
            // Filter nav commands
            List<ushort> navCmdIds = new List<ushort>
            {
                (ushort)MAV_CMD.WAYPOINT,
                (ushort)MAV_CMD.RETURN_TO_LAUNCH
            };

            ushort rtlSeq = flightPlan.Count(c => c.Value.command == (ushort)MAV_CMD.RETURN_TO_LAUNCH) > 0 ?
                flightPlan.Where(c => c.Value.command == (ushort)MAV_CMD.RETURN_TO_LAUNCH).Min(c => c.Value.seq) :
                ushort.MaxValue;

            Dictionary<int, mavlink_mission_item_int_t> nextNavOnly = (Dictionary<int, mavlink_mission_item_int_t>)flightPlan
                .Where(c => c.Value.seq >= nextWP
                    && c.Value.seq <= rtlSeq
                    && navCmdIds.Contains(c.Value.command))
                .OrderBy(c => c.Value.seq);

            // Create segments
            List<(PointLatLngAlt pos1, PointLatLngAlt pos2)> segments = new List<(PointLatLngAlt pos1, PointLatLngAlt pos2)>();
            foreach (KeyValuePair<int, mavlink_mission_item_int_t> missionItem in nextNavOnly)
            {
                // uavLocation -> WPnext
                if (missionItem.Value.seq == nextWP && rtlSeq != nextWP)
                {

                }

                // WPn -> WPn + 1
                // WPlast -> Home (if RTL)
            }

            return segments.Sum(s => PointsToAh(s.pos1, s.pos2, travelSpeed, travelConsumption, climbSpeed, climbConsumption, descSpeed, descConsumption));
        }
        #endregion
    }
}
