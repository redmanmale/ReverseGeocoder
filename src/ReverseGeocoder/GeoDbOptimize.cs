using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ReverseGeocoding.Model;

namespace ReverseGeocoding
{
    public static class GeoDbOptimize
    {
        public delegate bool Filter(string[] fields);

        public static OptStats OptimizeDatabase(Stream input,
                                                Stream output,
                                                IEnumerable<FeatureClass> featureClassFilter,
                                                IEnumerable<string> countryCodeFilter,
                                                List<List<PointD>> areaFilter = null)
        {
            long recordsIn = 0;
            long recordsOut = 0;

            var sb = new StringBuilder();
            foreach (var f in featureClassFilter)
            {
                sb.Append(f.ToFeatureCode());
            }

            var featureClassFilterCompiled = sb.ToString();
            var countryFilterCompiled = string.Join("|", countryCodeFilter);

            using (var reader = new StreamReader(input))
            using (var writer = new StreamWriter(output))
            {
                string line;
                while (!reader.EndOfStream && (line = reader.ReadLine()) != null)
                {
                    recordsIn++;
                    if (!FilterRecord(line.Split('\t'), featureClassFilterCompiled, countryFilterCompiled, areaFilter))
                    {
                        continue;
                    }

                    recordsOut++;
                    writer.WriteLine(line);
                }
            }

            return new OptStats(recordsIn, recordsOut);
        }

        public static OptStats OptimizeDatabase(Stream input, Stream output, Filter filter)
        {
            long recordsIn = 0;
            long recordsOut = 0;

            using (var reader = new StreamReader(input))
            using (var writer = new StreamWriter(output))
            {
                string line;
                while (!reader.EndOfStream && (line = reader.ReadLine()) != null)
                {
                    recordsIn++;
                    if (!filter(line.Split('\t')))
                    {
                        continue;
                    }

                    recordsOut++;
                    writer.WriteLine(line);
                }
            }

            return new OptStats(recordsIn, recordsOut);
        }

        private static double Angle2D(PointD point1, PointD point2)
        {
            var theta1 = Math.Atan2(point1.Y, point1.X);
            var theta2 = Math.Atan2(point2.Y, point2.X);
            var delta = theta2 - theta1;

            if (delta > Math.PI)
            {
                delta -= Math.PI + Math.PI;
            }
            else if (delta < -Math.PI)
            {
                delta += Math.PI + Math.PI;
            }
            return delta;
        }

        private static bool FilterRecord(string[] fields,
                                         string featureClassFilter,
                                         string countryCodeFilter,
                                         IEnumerable<List<PointD>> areaFilter = null)
        {
            if (featureClassFilter.IndexOf(fields.GetValue(PlaceInfoFields.FeatureClass), StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            if (countryCodeFilter.IndexOf(fields.GetValue(PlaceInfoFields.CountryCode), StringComparison.OrdinalIgnoreCase) == -1)
            {
                return false;
            }

            if (areaFilter == null)
            {
                return false;
            }

            var latitude = double.Parse(fields.GetValue(PlaceInfoFields.Latitude), CultureInfo.InvariantCulture);
            var longitude = double.Parse(fields.GetValue(PlaceInfoFields.Longitude), CultureInfo.InvariantCulture);
            var latLng = new PointD(latitude, longitude);
            return areaFilter.All(poly => PointInPolyManifold(latLng, poly));
        }

        //copied from: http://stackoverflow.com/questions/10673740/how-to-check-if-a-point-x-y-is-inside-a-polygon-in-the-cartesian-coordinate-sy
        private static bool PointInPolyManifold(PointD point, IReadOnlyList<PointD> poly)
        {
            double angle = 0;
            var size = poly.Count;
            for (var i = 0; i < size; i++)
            {
                angle += Angle2D(poly[i] - point, poly[(i + 1) % size] - point);
            }

            return !(Math.Abs(angle) < Math.PI);
        }
    }
}
