using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSharp
{
    public class CountryInfo : IEquatable<string>, IEquatable<CountryInfo>
    {
        public const int FieldCount = 19;

        public string ISO { get; set; }
        public string ISO3 { get; set; }
        public int ISONumeric { get; set; }
        public string Fips { get; set; }
        public string Country { get; set; }
        public string Capital { get; set; }
        public double Area { get; set; }
        public int Population { get; set; }
        public string Continent { get; set; }
        public string TopLevelDomain { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Phone { get; set; }
        public string PostalCodeFormat { get; set; }
        public string PostalCodeRegex { get; set; }
        public string Languages { get; set; }
        public int GeoNameId { get; set; }
        public string Neighbours { get; set; }
        public string EquivalentFipsCode { get; set; }

        public CountryInfo(string countryInfoEntry)
        {
            string[] Fields = countryInfoEntry.Split('\t');
            if (Fields.Length != FieldCount)
                throw new ArgumentException("Invalid countryInfo Record");
            int i = 0;
            this.ISO = Fields[i++];
            this.ISO3 = Fields[i++];
            this.ISONumeric = int.Parse(Fields[i++]);
            this.Fips = Fields[i++];
            this.Country = Fields[i++];
            this.Capital = Fields[i++];
            this.Area = double.Parse(Fields[i++]);
            this.Population = int.Parse(Fields[i++]);
            this.Continent = Fields[i++];
            this.TopLevelDomain = Fields[i++];
            this.CurrencyCode = Fields[i++];
            this.CurrencyName = Fields[i++];
            this.Phone = Fields[i++];
            this.PostalCodeFormat = Fields[i++];
            this.PostalCodeRegex = Fields[i++];
            this.Languages = Fields[i++];
            this.GeoNameId = int.Parse(Fields[i++]);
            this.Neighbours = Fields[i++];
            this.EquivalentFipsCode = Fields[i++];
        }

        public override string ToString()
        {
            return this.Country;
        }

        public override int GetHashCode()
        {
            return this.ISO.GetHashCode();
        }

        public bool Equals(string other)
        {
            return other == this.ISO;
        }

        public bool Equals(CountryInfo other)
        {
            return other.ISO == this.ISO;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is CountryInfo)
            {
                return (obj as CountryInfo).ISO == this.ISO;
            }
            else
                return base.Equals(obj);
        }
    }
}