using MissionPlanner.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
            public double MaxAmpHours;
            public double MinAmpHours;
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
        /// Container for wind info
        /// </summary>
        public struct WindInfo
        {
            public double Heading;
            public double Speed;
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

        private static double DEG_TO_RAD = Math.PI / 180.0;
        #endregion

        #region Methods
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

            return (powerInfo.MaxAmpHours / (powerInfo.MaxVolts - powerInfo.MinVolts) * (powerInfo.CurrentVolts - powerInfo.MinVolts)) - powerInfo.MinAmpHours;
        }

        private static double PointsToAh(PointLatLngAlt pos1, PointLatLngAlt pos2, UAVInfo uavInfo, WindInfo windInfo)
        {
            // Horizontal component
            double horizontalDist = pos1.GetDistance(pos2); // m
            double horizontalTime = horizontalDist / uavInfo.TravelSpeed; // s

            double heading = pos1.GetBearing(pos2);
            double windParallel = windInfo.Speed * Math.Cos((windInfo.Heading * DEG_TO_RAD) - (heading * DEG_TO_RAD));
            double windPerpendicular = windInfo.Speed * Math.Sin((windInfo.Heading * DEG_TO_RAD) - (heading * DEG_TO_RAD));
            double windyTravelSpeed = Math.Sqrt(Math.Pow(uavInfo.TravelSpeed + windParallel, 2) + Math.Pow(windPerpendicular, 2));
            double windyTravelConsumption = uavInfo.TravelConsumption * (windyTravelSpeed / uavInfo.TravelSpeed); // Ah
            
            double horizontalConsumption = horizontalTime * S_TO_H * windyTravelConsumption; // Ah

            // Vertical component
            double verticalDist = Math.Abs(pos1.Alt - pos2.Alt),    // m
                   verticalTime,                                    // s
                   verticalConsumption;                             // A
            if (pos1.Alt < pos2.Alt) // Climbing
            {
                verticalTime = verticalDist / uavInfo.ClimbSpeed;                       // s
                verticalConsumption = verticalTime * S_TO_H * uavInfo.ClimbConsumption; // Ah
            }
            else // Descending
            {
                verticalTime = verticalDist / uavInfo.DescSpeed;                        // s
                verticalConsumption = verticalTime * S_TO_H * uavInfo.DescConsumption;  // Ah
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
        public static double RequiredAh(FlightPlanInfo flightPlanInfo, UAVInfo uavInfo, WindInfo windInfo, int safetyMarginPercentage = 0)
        {
            // Clean flightplan
            CleanFlightplan(flightPlanInfo);

            // Check input data
            if (!IsFlightPlanInfoCorrect(flightPlanInfo)
                || !IsUAVInfoCorrect(uavInfo)
                || !IsWindInfoCorrect(windInfo)
                || safetyMarginPercentage < 0
                || safetyMarginPercentage > 100)
            {
                return double.MaxValue;
            }

            // Filter & sort nav commands
            List<ushort> navCmdIds = new List<ushort>
            {
                (ushort)MAV_CMD.WAYPOINT
            };

            ushort rtlSeq = flightPlanInfo.FlightPlan.Count(c => c.Value.command == (ushort)MAV_CMD.RETURN_TO_LAUNCH) > 0 ?
                flightPlanInfo.FlightPlan.Where(c => c.Value.command == (ushort)MAV_CMD.RETURN_TO_LAUNCH).Min(c => c.Value.seq) :
                ushort.MaxValue;

            List<MissionItem> nextNavOnly = new List<MissionItem>();
            foreach (KeyValuePair<int, mavlink_mission_item_int_t> missionItem in flightPlanInfo.FlightPlan)
            {
                if (missionItem.Value.seq >= flightPlanInfo.NextWP
                    && missionItem.Value.seq < rtlSeq // Do not include RTL
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
                    default: break;
                }
            }
            segments.Add((lastPos, flightPlanInfo.HomeLocation)); // Add RTL at the end

            // Calculate & sum segment consumptions (+ safety margin)
            return segments.Sum(s => PointsToAh(s.pos1, s.pos2, uavInfo, windInfo)) * (1 + ((double)safetyMarginPercentage / 100));
        }

        private static bool IsPowerInfoCorrect(PowerInfo powerInfo)
        {
            return 0 < powerInfo.MinVolts
                && powerInfo.MinVolts <= powerInfo.CurrentVolts + 0.001
                && powerInfo.CurrentVolts <= powerInfo.MaxVolts + 0.2
                && 0 < powerInfo.MaxAmpHours
                && 0 <= powerInfo.MinAmpHours
                && powerInfo.MinAmpHours < powerInfo.MaxAmpHours;
        }

        private static bool IsPositionCorrect(PointLatLngAlt position)
        {
            return Math.Abs(position.Lat) <= 90
                && Math.Abs(position.Lng) <= 180;
        }

        private static bool IsFlightPlanInfoCorrect(FlightPlanInfo flightPlanInfo)
        {
            return null != flightPlanInfo.FlightPlan
                && 0 < flightPlanInfo.FlightPlan.Count
                && 0 < flightPlanInfo.FlightPlan.Count(c => c.Value.seq == flightPlanInfo.NextWP)
                && null != flightPlanInfo.UAVLocation
                && IsPositionCorrect(flightPlanInfo.UAVLocation)
                && null != flightPlanInfo.HomeLocation
                && IsPositionCorrect(flightPlanInfo.HomeLocation);
        }

        private static void CleanFlightplan(FlightPlanInfo flightPlanInfo)
        {
            // Remove home
            flightPlanInfo.FlightPlan.TryRemove(0, out _);

            // Remove zero wps
            List<int> keysToRemove = flightPlanInfo.FlightPlan
                .Where(wp => (wp.Value.x + wp.Value.y + wp.Value.z) == 0)
                .Select(wp => wp.Key)
                .ToList();
            foreach (int key in keysToRemove)
            {
                flightPlanInfo.FlightPlan.TryRemove(key, out _);
            }

            // Set next wp index
            flightPlanInfo.NextWP = flightPlanInfo.FlightPlan.Min(wp => wp.Key);
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

        private static bool IsWindInfoCorrect(WindInfo windInfo)
        {
            return windInfo.Heading >= 0
                && windInfo.Heading < 360 // 360° = 0°
                && windInfo.Speed >= 0;
        }

        /// <summary>
        /// Determines if there is enough power in the UAV battery to execute the given flightplan
        /// </summary>
        /// <param name="powerInfo">UAV battery info container</param>
        /// <param name="flightPlanInfo">Flightplan info container</param>
        /// <param name="uavInfo">UAV speed & power consumption info container</param>
        /// <param name="safetyMarginPercentage">Safety margin to add to the required power in percentages(%)</param>
        /// <returns>True, if there is enough power in the UAV battery to execute the given flightplan</returns>
        public static bool IsFlightPlanPossible(PowerInfo powerInfo, FlightPlanInfo flightPlanInfo, UAVInfo uavInfo, WindInfo windInfo, int safetyMarginPercentage = 0)
        {
            // Calculate
            double requiredAh = RequiredAh(flightPlanInfo, uavInfo, windInfo, safetyMarginPercentage);
            double availableAh = AvailableAh(powerInfo);

            // Return
            return availableAh >= requiredAh;
        }
        #endregion
    }
}
