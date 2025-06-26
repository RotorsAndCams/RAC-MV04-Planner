using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Projections.Transforms;
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

        public static PointLatLng OffsetPoint(PointLatLng origin, double distanceMeters, double bearingDeg)
        {
            const double R = 6378137; // Earth radius in meters (WGS-84)
            double bearingRad = bearingDeg * Math.PI / 180.0;

            double lat1 = origin.Lat * Math.PI / 180.0;
            double lon1 = origin.Lng * Math.PI / 180.0;

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(distanceMeters / R) +
                                    Math.Cos(lat1) * Math.Sin(distanceMeters / R) * Math.Cos(bearingRad));

            double lon2 = lon1 + Math.Atan2(Math.Sin(bearingRad) * Math.Sin(distanceMeters / R) * Math.Cos(lat1),
                                            Math.Cos(distanceMeters / R) - Math.Sin(lat1) * Math.Sin(lat2));

            return new PointLatLng(lat2 * 180.0 / Math.PI, lon2 * 180.0 / Math.PI);
        }

        public static double HaversineDistance(PointLatLng p1, PointLatLng p2)
        {
            const double R = 6371000; // Earth's radius in meters

            double lat1 = p1.Lat;
            double lon1 = p1.Lng;
            double lat2 = p2.Lat;
            double lon2 = p2.Lng;

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in meters
        }
        private static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
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
            //PointLatLng copyLocation = new PointLatLng(currentLocation.Lat, currentLocation.Lng);

            // offset in m along bearing (void function)
            //copyLocation.Offset(horizontalDistance, bearingDeg);

            PointLatLng impactPoint = OffsetPoint(currentLocation, horizontalDistance, bearingDeg);

            return impactPoint;
        }

        public static double Bearing(PointLatLng p1, PointLatLng p2)
        {
            double lat1 = p1.Lat * Math.PI / 180.0;
            double lon1 = p1.Lng * Math.PI / 180.0;
            double lat2 = p2.Lat * Math.PI / 180.0;
            double lon2 = p2.Lng * Math.PI / 180.0;

            double dLon = lon2 - lon1;
            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) -
                       Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);

            double brng = Math.Atan2(y, x);
            return (brng * 180.0 / Math.PI + 360) % 360;
        }
    }
}
