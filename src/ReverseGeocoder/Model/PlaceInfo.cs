using System;
using System.Collections.Generic;
using System.Globalization;
using ReverseGeocoding.Interface;
using ReverseGeocoding.KdTree;

namespace ReverseGeocoding.Model
{
    internal class PlaceInfo : IKdComparator<PlaceInfo>, IPlaceInfo
    {
        private const char fieldsDelimiter = '\t';
        private const int fieldCount = 19;
        private const string modificationDateFormat = "yyyy-MM-dd";

        public int PlaceInfoId { get; }

        public string Name { get; }

        public string AsciiName { get; }

        public string AlternateNames { get; }

        public double Latitude { get; }

        public double Longitude { get; }

        public FeatureClass FeatureClass { get; }

        public string FeatureCode { get; }

        public string CountryCode { get; }

        public string AltCountryCodes { get; }

        public string Admin1Code { get; }

        public string Admin2Code { get; }

        public string Admin3Code { get; }

        public string Admin4Code { get; }

        public long Population { get; }

        public int? Elevation { get; }

        public int Dem { get; }

        public string TimeZone { get; }

        public DateTime ModificationDate { get; }

        public ICountryInfo CountryInfo { get; internal set; }

        public PlaceInfo(string geoEntry)
        {
            var fields = geoEntry.Split(fieldsDelimiter);
            if (fields.Length != fieldCount)
            {
                throw new ArgumentException("Invalid GeoName Record");
            }

            PlaceInfoId = int.Parse(fields.GetValue(PlaceInfoFields.PlaceInfoId), CultureInfo.InvariantCulture);
            Name = fields.GetValue(PlaceInfoFields.Name);
            AsciiName = fields.GetValue(PlaceInfoFields.AsciiName);
            AlternateNames = fields.GetValue(PlaceInfoFields.AlternateNames);
            Latitude = double.Parse(fields.GetValue(PlaceInfoFields.Latitude), CultureInfo.InvariantCulture);
            Longitude = double.Parse(fields.GetValue(PlaceInfoFields.Longitude), CultureInfo.InvariantCulture);
            FeatureCode = fields.GetValue(PlaceInfoFields.FeatureCode);
            FeatureClass = FeatureCode.ToFeatureClass();
            CountryCode = fields.GetValue(PlaceInfoFields.CountryCode);
            AltCountryCodes = fields.GetValue(PlaceInfoFields.AltCountryCodes);
            Admin1Code = fields.GetValue(PlaceInfoFields.Admin1Code);
            Admin2Code = fields.GetValue(PlaceInfoFields.Admin2Code);
            Admin3Code = fields.GetValue(PlaceInfoFields.Admin3Code);
            Admin4Code = fields.GetValue(PlaceInfoFields.Admin4Code);
            Population = long.Parse(fields.GetValue(PlaceInfoFields.Population), CultureInfo.InvariantCulture);
            Elevation = fields.GetValue(PlaceInfoFields.Elevation).GetIntOrNull();
            Dem = int.Parse(fields.GetValue(PlaceInfoFields.Dem), CultureInfo.InvariantCulture);
            TimeZone = fields.GetValue(PlaceInfoFields.TimeZone);
            ModificationDate = DateTime.ParseExact(fields.GetValue(PlaceInfoFields.ModificationDate), modificationDateFormat, CultureInfo.InvariantCulture);
        }

        public PlaceInfo(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        double IKdComparator<PlaceInfo>.AxisSquaredDistance(PlaceInfo location, Axis axis)
        {
            double distance;
            switch (axis)
            {
                case Axis.X:
                    distance = X - location.X;
                    break;
                case Axis.Y:
                    distance = Y - location.Y;
                    break;
                case Axis.Z:
                    distance = Z - location.Z;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }

            return distance * distance;
        }

        IComparer<PlaceInfo> IKdComparator<PlaceInfo>.Comparator(Axis axis)
        {
            switch (axis)
            {
                case Axis.X: return new CompareX();
                case Axis.Y: return new CompareY();
                case Axis.Z: return new CompareZ();
                default: throw new ArgumentOutOfRangeException(nameof(axis), axis, "Invalid Axis");
            }
        }

        // The following methods are used purely for the KD-Tree
        // They don't convert lat/lon to any particular coordinate system
        private double X => Math.Cos(Deg2Rad(Latitude)) * Math.Cos(Deg2Rad(Longitude));
        private double Y => Math.Cos(Deg2Rad(Latitude)) * Math.Sin(Deg2Rad(Longitude));
        private double Z => Math.Sin(Deg2Rad(Latitude));

        double IKdComparator<PlaceInfo>.SquaredDistance(PlaceInfo location)
        {
            var x = X - location.X;
            var y = Y - location.Y;
            var z = Z - location.Z;
            return x * x + y * y + z * z;
        }

        public override string ToString()
        {
            return Name;
        }

        private static double Deg2Rad(double deg) => deg * Math.PI / 180.0;

        private class CompareX : IComparer<PlaceInfo>
        {
            public int Compare(PlaceInfo x, PlaceInfo y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                return y == null ? 1 : x.X.CompareTo(y.X);
            }
        }

        private class CompareY : IComparer<PlaceInfo>
        {
            public int Compare(PlaceInfo x, PlaceInfo y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                return y == null ? 1 : x.Y.CompareTo(y.Y);
            }
        }

        private class CompareZ : IComparer<PlaceInfo>
        {
            public int Compare(PlaceInfo x, PlaceInfo y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                return y == null ? 1 : x.Z.CompareTo(y.Z);
            }
        }
    }
}
