using System;
namespace SmartList
{
    public static class DistanceHelper
    {
        public static double DistanceBetween (double latitude1, double longitude1, double latitude2, double longitude2)
        {
            // Based on http://slodge.blogspot.ca/2012/04/calculating-distance-between-latlng.html
            if (latitude1 == latitude2 && longitude1 == longitude2)
                return 0.0;

            var theta = longitude1 - longitude2;

            var distance = Math.Sin (deg2rad (latitude1)) * Math.Sin (deg2rad (latitude2)) +
                           Math.Cos (deg2rad (latitude1)) * Math.Cos (deg2rad (latitude2)) *
                           Math.Cos (deg2rad (theta));

            distance = Math.Acos (distance);
            if (double.IsNaN (distance))
                return 0.0;

            distance = rad2deg (distance);
            distance = distance * 60.0 * 1.1515 * 1609.344;

            return (distance);
        }

        private static double deg2rad (double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg (double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
