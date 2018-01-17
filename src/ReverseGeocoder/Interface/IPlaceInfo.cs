using System;
using ReverseGeocoding.Model;

namespace ReverseGeocoding.Interface
{
    public interface IPlaceInfo
    {
        int PlaceInfoId { get; }

        string Name { get; }

        string AsciiName { get; }

        string AlternateNames { get; }

        double Latitude { get; }

        double Longitude { get; }

        FeatureClass FeatureClass { get; }

        string FeatureCode { get; }

        string CountryCode { get; }

        string AltCountryCodes { get; }

        string Admin1Code { get; }

        string Admin2Code { get; }

        string Admin3Code { get; }

        string Admin4Code { get; }

        long Population { get; }

        int? Elevation { get; }

        int Dem { get; }

        string TimeZone { get; }

        DateTime ModificationDate { get; }

        ICountryInfo CountryInfo { get; }
    }
}
