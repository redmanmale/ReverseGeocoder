using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeoSharp
{
    /// <summary>
    /// All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
    /// </summary>
    public class GeoDB
    {
        public List<GeoName> Places { get; set; }

        public List<CountryInfo> CountryInfos { get; set; }

        public GeoDB()
        {
            Places = new List<GeoName>();
            CountryInfos = new List<CountryInfo>();
        }

        /// <summary>
        /// All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
        /// </summary>
        /// <param name="input"></param>
        public void SetPlaces(Stream input)
        {
            Places.Clear();
            using (StreamReader db = new StreamReader(input))
            {
                string line;
                while (!db.EndOfStream && (line = db.ReadLine()) != null)
                {
                    if (line.StartsWith("#")) continue;
                    var place = new GeoName(line, CountryInfos);
                    Places.Add(place);
                }
            }
        }

        /// <summary>
        /// All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
        /// </summary>
        /// <param name="placesDatabase"></param>
        public void SetPlaces(string placesDatabase)
        {
            using (var db = new FileStream(placesDatabase, FileMode.Open, FileAccess.Read))
            {
                SetPlaces(db);
            }
        }

        /// <summary>
        /// All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
        /// </summary>
        /// <param name="input"></param>
        public void SetCountryInfos(Stream input)
        {
            CountryInfos.Clear();
            using (StreamReader db = new StreamReader(input))
            {
                string line;
                while (!db.EndOfStream && (line = db.ReadLine()) != null)
                {
                    if (line.StartsWith("#")) continue;
                    var countryInfo = new CountryInfo(line);
                    CountryInfos.Add(countryInfo);
                }
            }
            if (Places.Any())
            {
                Places.ForEach(p => p.Country = CountryInfos.Find(c => c.ISO == p.CountryCode));
            }
        }

        /// <summary>
        /// All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
        /// </summary>
        /// <param name="countryDatabase"></param>
        public void SetCountryInfos(string countryDatabase)
        {
            using (var db = new FileStream(countryDatabase, FileMode.Open, FileAccess.Read))
            {
                SetCountryInfos(db);
            }
        }
    }
}