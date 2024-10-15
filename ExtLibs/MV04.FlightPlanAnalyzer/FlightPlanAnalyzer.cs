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
        #region Types
        private struct MissionItem
        {
            public int Seq;
            public MAV_CMD Command;
            public double Lat;
            public double Lng;
            public double Alt;
        }

        /// <summary>
        /// Container for UAV battery info
        /// </summary>
        public struct PowerInfo
        {
            public double CurrentVolts;
            public double MaxVolts;
            public double MinVolts;
            public double FullAmpHours;
        }

        /// <summary>
        /// Container for UAV speed & power consumption info
        /// </summary>
        public struct UAVInfo
        {
            public double TravelSpeed;
            public double TravelConsumption;
            public double ClimbSpeed;
            public double ClimbConsumption;
            public double DescSpeed;
            public double DescConsumption;
        }

        /// <summary>
        /// Contaner for flightplan info
        /// </summary>
        public struct FlightPlanInfo
        {
            public ConcurrentDictionary<int, mavlink_mission_item_int_t> FlightPlan;
            public int NextWP;
            public PointLatLngAlt UAVLocation;
            public PointLatLngAlt HomeLocation;
        }
        #endregion

        #region Fields
        private static readonly double H_TO_S = 3600.0;

        private static readonly double S_TO_H = 1.0 / H_TO_S;

        /*private static (double AirSpeed, double Consumption)[] AirSpeedConsumptionTable =
        {
            // TODO: Populate AirSpeedConsumptionTable with measurements
            (10, 5),
            (20, 12)
        };*/
        #endregion

        #region Methods
        /*private static double AirSpeedToAh(double airSpeed)
        {
            double slope = 0;
            for (int i = 0; i < AirSpeedConsumptionTable.Length; i++)
            {
                slope += AirSpeedConsumptionTable[i].Consumption / AirSpeedConsumptionTable[i].AirSpeed;
            }
            slope /= AirSpeedConsumptionTable.Length;

            return airSpeed * slope;
        }*/

        /// <summary>
        /// Calculate the available power in the UAV battery
        /// </summary>
        /// <param name="powerInfo">Power info container</param>
        /// <returns>Amp-hours available</returns>
        public static double AvailableAh(PowerInfo powerInfo)
        {
            if (!IsPowerInfoCorrect(powerInfo))
            {
                return 0;
            }

            return powerInfo.FullAmpHours / (powerInfo.MaxVolts - powerInfo.MinVolts) * (powerInfo.CurrentVolts - powerInfo.MinVolts);
            // TODO: Replace BatteryVoltsToAh with more precise (non-linear) function
        }

        private static double GetDistance(PointLatLngAlt pos1, PointLatLngAlt pos2)
        {
            // Convert degrees to radians
            double radLat1 = Math.PI * pos1.Lat / 180;
            double radLat2 = Math.PI * pos2.Lat / 180;
            double radLonDiff = Math.PI * (pos2.Lng - pos1.Lng) / 180;

            // Haversine formula
            double haversine = Math.Pow(Math.Sin((radLat2 - radLat1) / 2), 2) +
                              Math.Cos(radLat1) * Math.Cos(radLat2) *
                              Math.Pow(Math.Sin(radLonDiff / 2), 2);

            // Earth's radius in meters
            double earthRadiusMeters = 6371000; // 6371 kilometers * 1000 meters/kilometer

            // Calculate distance in meters
            double distanceMeters = 2 * earthRadiusMeters * Math.Asin(Math.Sqrt(haversine));

            return distanceMeters;
        }

        private static double PointsToAh(PointLatLngAlt pos1, PointLatLngAlt pos2, double travelSpeed, double travelConsumption, double climbSpeed, double climbConsumption, double descSpeed, double descConsumption)
        {
            // Horizontal component
            double horizontalDist = GetDistance(pos1, pos2);                            // m
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

        /// <summary>
        /// Calculate the required power to execute the given flightplan
        /// </summary>
        /// <param name="flightPlanInfo">Flightplan info container</param>
        /// <param name="uavInfo">UAV info container</param>
        /// <param name="safetyMarginPercentage">Safety margin to add to result in percentages(%)</param>
        /// <returns>Amp-hours required</returns>
        public static double RequiredAh(FlightPlanInfo flightPlanInfo, UAVInfo uavInfo, int safetyMarginPercentage = 0)
        {
            // Check input data
            if (!IsFlightPlanInfoCorrect(flightPlanInfo)
                || !IsUAVInfoCorrect(uavInfo)
                || safetyMarginPercentage < 0
                || safetyMarginPercentage > 100)
            {
                return double.MaxValue;
            }
            if (flightPlanInfo.NextWP == 0) flightPlanInfo.NextWP = 1; // Home WP was removed in IsFlightPlanInfoCorrect()

            // Filter & sort nav commands
            List<ushort> navCmdIds = new List<ushort>
            {
                (ushort)MAV_CMD.WAYPOINT,
                (ushort)MAV_CMD.RETURN_TO_LAUNCH
            };

            ushort rtlSeq = flightPlanInfo.FlightPlan.Count(c => c.Value.command == (ushort)MAV_CMD.RETURN_TO_LAUNCH) > 0 ?
                flightPlanInfo.FlightPlan.Where(c => c.Value.command == (ushort)MAV_CMD.RETURN_TO_LAUNCH).Min(c => c.Value.seq) :
                ushort.MaxValue;

            List<MissionItem> nextNavOnly = new List<MissionItem>();
            foreach (KeyValuePair<int, mavlink_mission_item_int_t> missionItem in flightPlanInfo.FlightPlan)
            {
                if (missionItem.Value.seq >= flightPlanInfo.NextWP
                    && missionItem.Value.seq <= rtlSeq
                    && navCmdIds.Contains(missionItem.Value.command))
                {
                    nextNavOnly.Add(new MissionItem()
                    {
                        Seq = missionItem.Value.seq,
                        Command = (MAV_CMD)missionItem.Value.command,
                        Lat = (double)missionItem.Value.x / 10000000,
                        Lng = (double)missionItem.Value.y / 10000000,
                        Alt = missionItem.Value.z
                    });
                }
            }
            nextNavOnly = nextNavOnly.OrderBy(c => c.Seq).ToList();

            // Create segments
            List<(PointLatLngAlt pos1, PointLatLngAlt pos2)> segments = new List<(PointLatLngAlt pos1, PointLatLngAlt pos2)>();
            PointLatLngAlt lastPos = flightPlanInfo.UAVLocation;
            foreach (MissionItem missionItem in nextNavOnly)
            {
                switch (missionItem.Command)
                {
                    case MAV_CMD.WAYPOINT:
                        PointLatLngAlt thisPos = new PointLatLngAlt(missionItem.Lat, missionItem.Lng, missionItem.Alt);
                        segments.Add((lastPos, thisPos));
                        lastPos = thisPos;
                        break;
                    case MAV_CMD.RETURN_TO_LAUNCH:
                        segments.Add((lastPos, flightPlanInfo.HomeLocation));
                        break;
                    default: break;
                }
            }

            // Calculate & sum segment consumptions (+ safety margin)
            return segments.Sum(s => PointsToAh(s.pos1, s.pos2, uavInfo.TravelSpeed, uavInfo.TravelConsumption, uavInfo.ClimbSpeed, uavInfo.ClimbConsumption, uavInfo.DescSpeed, uavInfo.DescConsumption)) * (1 + ((double)safetyMarginPercentage / 100));
        }

        private static bool IsPowerInfoCorrect(PowerInfo powerInfo)
        {
            return 0 < powerInfo.MinVolts
                && powerInfo.MinVolts <= powerInfo.CurrentVolts + 0.001
                && powerInfo.CurrentVolts <= powerInfo.MaxVolts + 0.2
                && 0 < powerInfo.FullAmpHours;
        }

        private static bool IsPositionCorrect(PointLatLngAlt position)
        {
            return Math.Abs(position.Lat) <= 90
                && Math.Abs(position.Lng) <= 180;
        }

        private static bool IsFlightPlanInfoCorrect(FlightPlanInfo flightPlanInfo)
        {
            flightPlanInfo.FlightPlan.TryRemove(0, out _);
            if (flightPlanInfo.NextWP == 0) flightPlanInfo.NextWP = 1;

            return null != flightPlanInfo.FlightPlan
                && 0 < flightPlanInfo.FlightPlan.Count
                && 0 < flightPlanInfo.FlightPlan.Count(c => c.Value.seq == flightPlanInfo.NextWP)
                && null != flightPlanInfo.UAVLocation
                && IsPositionCorrect(flightPlanInfo.UAVLocation)
                && null != flightPlanInfo.HomeLocation
                && IsPositionCorrect(flightPlanInfo.HomeLocation);
        }

        private static bool IsUAVInfoCorrect(UAVInfo uavInfo)
        {
            return 0 < uavInfo.TravelSpeed
                && 0 < uavInfo.TravelConsumption
                && 0 < uavInfo.ClimbSpeed
                && 0 < uavInfo.ClimbConsumption
                && 0 < uavInfo.DescSpeed
                && 0 < uavInfo.DescConsumption;
        }

        /// <summary>
        /// Determines if there is enough power in the UAV battery to execute the given flightplan
        /// </summary>
        /// <param name="powerInfo">UAV battery info container</param>
        /// <param name="flightPlanInfo">Flightplan info container</param>
        /// <param name="uavInfo">UAV speed & power consumption info container</param>
        /// <param name="safetyMarginPercentage">Safety margin to add to the required power in percentages(%)</param>
        /// <returns>True, if there is enough power in the UAV battery to execute the given flightplan</returns>
        public static bool IsFlightPlanPossible(PowerInfo powerInfo, FlightPlanInfo flightPlanInfo, UAVInfo uavInfo, int safetyMarginPercentage = 0)
        {
            // Calculate
            double requiredAh = RequiredAh(flightPlanInfo, uavInfo, safetyMarginPercentage);
            double availableAh = AvailableAh(powerInfo);

            // Return
            return availableAh >= requiredAh;
        }
        #endregion
    }
}
