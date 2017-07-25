using ReverseGeocoder.Model;

namespace ReverseGeocoder.Interface
{
    public interface IPlaceInfo
    {
        string CountryCode { get; }
        ICountryInfo CountryInfo { get; }
        FeatureClass FeatureClass { get; }
        double Latitude { get; }
        double Longitude { get; }
        string Name { get; }
    }
}
