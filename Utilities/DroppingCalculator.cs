using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMap.NET;

namespace MissionPlanner.Utilities
{
    public static class DroppingCalculator
    {
        private const double g = 9.81;
        public static double ComputeFallTime(double altitudeAGL)
        {
            if (altitudeAGL <= 0) return 0.0;
            return Math.Sqrt(2.0 * altitudeAGL / g);
        }

        public static double ComputeHorizontalDistance(double vHoriz, double fallTime)
        {
            return vHoriz * fallTime;
        }

        public static PointLatLng ComputeImpactPoint(
            PointLatLng currentLocation,
            double altitudeAGL,
            double vHoriz,
            double bearingDeg)
        {
            // Time to fall
            double tFall = ComputeFallTime(altitudeAGL);

            // Horizontal distance
            double horizontalDistance = ComputeHorizontalDistance(vHoriz, tFall);
            System.Diagnostics.Debug.WriteLine("horizontalDistance: " + horizontalDistance);

            // Copy of the currentLocation (to not modify the caller's)
            PointLatLng copyLocation = new PointLatLng(currentLocation.Lat, currentLocation.Lng);

            // offset in m along bearing (void function)
            copyLocation.Offset(horizontalDistance, bearingDeg);

            return copyLocation;
        }
    }
}
