using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeoSharp
{
    public struct OptStats
    {
        public long RecordsIn;
        public long RecordsOut;

        public OptStats(long In, long Out)
        {
            RecordsIn = In;
            RecordsOut = Out;
        }
    }

    public struct PointD //why doesn't .Net have a non-windows specific version of this :'(
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static PointD operator -(PointD a, PointD b)
        {
            return new PointD(a.X - b.X, a.Y - b.Y);
        }
    }

    public class GeoDBOptimize
    {
        public delegate bool Filter(string[] fields);

        public static OptStats OptimizeDatabase(Stream input, Stream output, List<GeoFeatureClass> featureClassFilter, List<string> countryFilter, List<List<PointD>> areaFilter)
        {
            long RecordsIn = 0;
            long RecordsOut = 0;
            string CountryFilterCompiled = string.Join("|", countryFilter.ToArray());
            StringBuilder sb = new StringBuilder();
            foreach (var f in featureClassFilter)
                sb.Append(GeoName.ClassToCode(f));

            string FeatureClassFilterCompiled = sb.ToString();

            using (StreamReader _in = new StreamReader(input))
            {
                using (StreamWriter _out = new StreamWriter(output))
                {
                    string Line;
                    while (!_in.EndOfStream && (Line = _in.ReadLine()) != null)
                    {
                        RecordsIn++;
                        if (FilterRecord(Line.Split('\t'), FeatureClassFilterCompiled, CountryFilterCompiled, areaFilter))
                        {
                            RecordsOut++;
                            _out.WriteLine(Line);
                        }
                    }
                }
            }

            return new OptStats(RecordsIn, RecordsOut);
        }

        public static OptStats OptimizeDatabase(Stream input, Stream output, List<GeoFeatureClass> featureClassFilter, List<string> countryFilter)
        {
            long RecordsIn = 0;
            long RecordsOut = 0;
            string CountryFilterCompiled = string.Join("|", countryFilter.ToArray());
            StringBuilder sb = new StringBuilder();
            foreach (var f in featureClassFilter)
                sb.Append(GeoName.ClassToCode(f));

            string FeatureClassFilterCompiled = sb.ToString();

            using (StreamReader _in = new StreamReader(input))
            {
                using (StreamWriter _out = new StreamWriter(output))
                {
                    string Line;
                    while (!_in.EndOfStream && (Line = _in.ReadLine()) != null)
                    {
                        RecordsIn++;
                        if (FilterRecord(Line.Split('\t'), FeatureClassFilterCompiled, CountryFilterCompiled))
                        {
                            RecordsOut++;
                            _out.WriteLine(Line);
                        }
                    }
                }
            }

            return new OptStats(RecordsIn, RecordsOut);
        }

        public static OptStats OptimizeDatabase(Stream input, Stream output, Filter filter)
        {
            long recordsIn = 0;
            long recordsOut = 0;

            using (StreamReader _in = new StreamReader(input))
            {
                using (StreamWriter _out = new StreamWriter(output))
                {
                    string line;
                    while (!_in.EndOfStream && (line = _in.ReadLine()) != null)
                    {
                        recordsIn++;
                        if (filter(line.Split('\t')))
                        {
                            recordsOut++;
                            _out.WriteLine(line);
                        }
                    }
                }
            }

            return new OptStats(recordsIn, recordsOut);
        }

        private static double Angle2D(PointD point1, PointD point2)
        {
            double theta1 = Math.Atan2(point1.Y, point1.X);
            double theta2 = Math.Atan2(point2.Y, point2.X);
            double delta = theta2 - theta1;

            if (delta > Math.PI)
                delta -= Math.PI + Math.PI;
            else if (delta < -Math.PI)
                delta += Math.PI + Math.PI;

            return delta;
        }

        private static bool FilterRecord(string[] fields, string featureClassFilter, string countryFilter, List<List<PointD>> areaFilter)
        {
            if (featureClassFilter.IndexOf(fields[6], StringComparison.Ordinal) == -1)
                return false;

            if (countryFilter.IndexOf(fields[8], StringComparison.Ordinal) == -1)
                return false;

            PointD LatLng = new PointD(double.Parse(fields[4]), double.Parse(fields[5]));
            foreach (var poly in areaFilter)
            {
                if (!PointInPolyManifold(LatLng, poly))
                    return false;
            }

            return true;
        }

        private static bool FilterRecord(string[] fields, string featureClassFilter, string countryFilter)
        {
            if (featureClassFilter.IndexOf(fields[6], StringComparison.Ordinal) == -1)
                return false;

            if (countryFilter.IndexOf(fields[8], StringComparison.Ordinal) == -1)
                return false;

            return true;
        }

        //copied from: http://stackoverflow.com/questions/10673740/how-to-check-if-a-point-x-y-is-inside-a-polygon-in-the-cartesian-coordinate-sy
        private static bool PointInPolyManifold(PointD point, List<PointD> poly)
        {
            double angle = 0;
            int size = poly.Count;
            for (int i = 0; i < size; i++)
            {
                angle += Angle2D(poly[i] - point, poly[(i + 1) % size] - point);
            }

            return Math.Abs(angle) < Math.PI ? false : true;
        }
    }
}