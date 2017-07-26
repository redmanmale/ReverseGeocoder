using System;
using System.Globalization;
using ReverseGeocoding.Interface;

namespace ReverseGeocoding.Model
{
    internal class CountryInfo : IEquatable<string>, IEquatable<CountryInfo>, ICountryInfo
    {
        private const int fieldCount = 19;

        public string Iso { get; }

        public string Iso3 { get; }

        public int IsoNumeric { get; }

        public string Fips { get; }

        public string Country { get; }

        public string Capital { get; }

        public double Area { get; }

        public int Population { get; }

        public string Continent { get; }

        public string TopLevelDomain { get; }

        public string CurrencyCode { get; }

        public string CurrencyName { get; }

        public string Phone { get; }

        public string PostalCodeFormat { get; }

        public string PostalCodeRegex { get; }

        public string Languages { get; }

        public int GeoNameId { get; }

        public string Neighbours { get; }

        public string EquivalentFipsCode { get; }

        public CountryInfo(string countryInfoEntry)
        {
            var fields = countryInfoEntry.Split('\t');
            if (fields.Length != fieldCount)
            {
                throw new ArgumentException("Invalid countryInfo Record");
            }

            Iso = fields.GetValue(CountryInfoFields.Iso);
            Iso3 = fields.GetValue(CountryInfoFields.Iso3);
            IsoNumeric = int.Parse(fields.GetValue(CountryInfoFields.IsoNumeric), CultureInfo.InvariantCulture);
            Fips = fields.GetValue(CountryInfoFields.Fips);
            Country = fields.GetValue(CountryInfoFields.Country);
            Capital = fields.GetValue(CountryInfoFields.Capital);
            Area = double.Parse(fields.GetValue(CountryInfoFields.Area), CultureInfo.InvariantCulture);
            Population = int.Parse(fields.GetValue(CountryInfoFields.Population), CultureInfo.InvariantCulture);
            Continent = fields.GetValue(CountryInfoFields.Continent);
            TopLevelDomain = fields.GetValue(CountryInfoFields.Tld);
            CurrencyCode = fields.GetValue(CountryInfoFields.CurrencyCode);
            CurrencyName = fields.GetValue(CountryInfoFields.CurrencyName);
            Phone = fields.GetValue(CountryInfoFields.Phone);
            PostalCodeFormat = fields.GetValue(CountryInfoFields.PostalCodeFormat);
            PostalCodeRegex = fields.GetValue(CountryInfoFields.PostalCodeRegex);
            Languages = fields.GetValue(CountryInfoFields.Languages);
            GeoNameId = int.Parse(fields.GetValue(CountryInfoFields.GeoNameId), CultureInfo.InvariantCulture);
            Neighbours = fields.GetValue(CountryInfoFields.Neighbours);
            EquivalentFipsCode = fields.GetValue(CountryInfoFields.EquivalentFipsCode);
        }

        public override string ToString()
        {
            return Country;
        }

        public override int GetHashCode()
        {
            return Iso.GetHashCode();
        }

        public bool Equals(string other)
        {
            return other == Iso;
        }

        public bool Equals(CountryInfo other)
        {
            return other?.Iso == Iso;
        }

        public override bool Equals(object obj)
        {
            if (obj is CountryInfo info)
            {
                return info.Iso == Iso;
            }

            return ReferenceEquals(this, obj);
        }
    }
}