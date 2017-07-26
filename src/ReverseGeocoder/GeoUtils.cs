using System;
using ReverseGeocoding.Model;

namespace ReverseGeocoding
{
    internal static class GeoUtils
    {
        public static FeatureClass ToFeatureClass(this string classCode)
        {
            if (string.IsNullOrWhiteSpace(classCode))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(classCode));
            }

            switch (classCode[0])
            {
                case 'A': return FeatureClass.Country;
                case 'P': return FeatureClass.City;
                case 'H': return FeatureClass.WaterBody;
                case 'L': return FeatureClass.LandArea;
                case 'R': return FeatureClass.TransportRoute;
                case 'S': return FeatureClass.Facility;
                case 'T': return FeatureClass.GeographicLandmark;
                case 'U': return FeatureClass.UnderseaLandmark;
                case 'V': return FeatureClass.Vegetation;
                default: throw new ArgumentException("Invalid Feature Class");
            }
        }

        public static char ToFeatureCode(this FeatureClass featureClass)
        {
            switch (featureClass)
            {
                case FeatureClass.Country: return 'A';
                case FeatureClass.City: return 'P';
                case FeatureClass.WaterBody: return 'H';
                case FeatureClass.LandArea: return 'L';
                case FeatureClass.TransportRoute: return 'R';
                case FeatureClass.Facility: return 'S';
                case FeatureClass.GeographicLandmark: return 'T';
                case FeatureClass.UnderseaLandmark: return 'U';
                case FeatureClass.Vegetation: return 'V';
                default: throw new ArgumentException("Invalid Feature Class");
            }
        }

        public static string GetValue<TEnum>(this string[] fields, TEnum field) where TEnum : struct, IConvertible
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum");
            }

            return fields[(int)Enum.ToObject(type, field)];
        }
    }
}
