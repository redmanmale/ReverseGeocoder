namespace ReverseGeocoding.Interface
{
    public interface ICountryInfo
    {
        double Area { get; }
        string Capital { get; }
        string Continent { get; }
        string Country { get; }
        string CurrencyCode { get; }
        string CurrencyName { get; }
        string EquivalentFipsCode { get; }
        string Fips { get; }
        int GeoNameId { get; }
        string Iso { get; }
        string Iso3 { get; }
        int IsoNumeric { get; }
        string Languages { get; }
        string Neighbours { get; }
        string Phone { get; }
        int Population { get; }
        string PostalCodeFormat { get; }
        string PostalCodeRegex { get; }
        string TopLevelDomain { get; }
    }
}
