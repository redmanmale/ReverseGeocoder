using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReverseGeocoding.Interface;
using ReverseGeocoding.Model;

namespace ReverseGeocoding
{
    /// <summary>
    /// All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
    /// </summary>
    public class GeoDb
    {
        private const string commentMark = "#";

        public IReadOnlyList<IPlaceInfo> Places { get; }

        public IReadOnlyList<ICountryInfo> CountryInfos { get; }

        public GeoDb(Stream placesDb, Stream countryInfoDb = null)
        {
            if (placesDb == null)
            {
                throw new ArgumentNullException(nameof(placesDb));
            }

            IReadOnlyDictionary<string, ICountryInfo> countryInfos = null;

            if (countryInfoDb != null)
            {
                countryInfos = GetCountryInfos(countryInfoDb);
                CountryInfos = countryInfos.Values.ToList();
            }

            Places = GetPlaces(placesDb, countryInfos);
        }

        public GeoDb(string placesDbPath, string countryInfoDbPath = null)
        {
            if (string.IsNullOrWhiteSpace(placesDbPath))
            {
                throw new ArgumentNullException(nameof(placesDbPath));
            }

            IReadOnlyDictionary<string, ICountryInfo> countryInfos = null;

            if (!string.IsNullOrWhiteSpace(countryInfoDbPath))
            {
                countryInfos = GetCountryInfos(countryInfoDbPath);
                CountryInfos = countryInfos.Values.ToList();
            }

            Places = GetPlaces(placesDbPath, countryInfos);
        }

        private static IReadOnlyList<IPlaceInfo> GetPlaces(Stream input, IReadOnlyDictionary<string, ICountryInfo> countryInfos)
        {
            var places = new List<IPlaceInfo>();
            using (var db = new StreamReader(input))
            {
                string line;
                while (!db.EndOfStream && (line = db.ReadLine()) != null)
                {
                    if (line.StartsWith(commentMark))
                    {
                        continue;
                    }

                    var place = new PlaceInfo(line);
                    if (countryInfos != null && !string.IsNullOrWhiteSpace(place.CountryCode) && countryInfos.ContainsKey(place.CountryCode))
                    {
                        place.CountryInfo = countryInfos[place.CountryCode];
                    }

                    places.Add(place);
                }
            }

            return places;
        }

        private static IReadOnlyList<IPlaceInfo> GetPlaces(string placesDbPath, IReadOnlyDictionary<string, ICountryInfo> countryInfos)
        {
            using (var stream = new FileStream(placesDbPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return GetPlaces(stream, countryInfos);
            }
        }

        private static IReadOnlyDictionary<string, ICountryInfo> GetCountryInfos(Stream input)
        {
            var countryInfos = new Dictionary<string, ICountryInfo>();
            using (var db = new StreamReader(input))
            {
                string line;
                while (!db.EndOfStream && (line = db.ReadLine()) != null)
                {
                    if (line.StartsWith(commentMark))
                    {
                        continue;
                    }

                    var countryInfo = new CountryInfo(line);
                    countryInfos.Add(countryInfo.Iso, countryInfo);
                }
            }

            return countryInfos;
        }

        private static IReadOnlyDictionary<string, ICountryInfo> GetCountryInfos(string countryDatabase)
        {
            using (var stream = new FileStream(countryDatabase, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return GetCountryInfos(stream);
            }
        }
    }
}