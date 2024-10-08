using MissionPlanner.Utilities;
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
            // TODO: Replace BatteryVoltsToAh with more precise (non-linear) function
            return fullAmpHours / (maxVolts - minVolts) * (currentVolts - minVolts);
        }

        private static double FPPathToAh(ConcurrentDictionary<int, mavlink_mission_item_int_t> flightPlan, Vector3D wind, PointLatLngAlt uavLocation)
        {
            // Get path vectors from flightplan
            // TODO: How many paths are there? (not all mission items are WPs, +1 for the current location -> closest WP)
            Vector3D[] Paths = new Vector3D[];

            return 0;
        }

        public static bool IsFlightPlanPossible(ConcurrentDictionary<int, mavlink_mission_item_int_t> plan, double batteryVoltage, Vector3D wind)
        {
            return false;
        }
        #endregion
    }
}
