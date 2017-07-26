using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReverseGeocoding.Interface;
using ReverseGeocoding.KdTree;
using ReverseGeocoding.Model;

namespace ReverseGeocoding
{
    public class ReverseGeocoder : IReverseGeocoder
    {
        private readonly KdTree<PlaceInfo> _tree;

        private ReverseGeocoder(IEnumerable<IPlaceInfo> geoNames) => _tree = new KdTree<PlaceInfo>(geoNames.Cast<PlaceInfo>().ToArray());

        public ReverseGeocoder(GeoDb geoDb) : this(geoDb.Places)
        {
            if (geoDb == null)
            {
                throw new ArgumentNullException(nameof(geoDb));
            }
        }

        public ReverseGeocoder(Stream placesDb, Stream countryInfoDb = null) : this(new GeoDb(placesDb, countryInfoDb))
        {
            if (placesDb == null)
            {
                throw new ArgumentNullException(nameof(placesDb));
            }
        }

        public ReverseGeocoder(string placesDbPath, string countryInfoDbPath = null) : this(new GeoDb(placesDbPath, countryInfoDbPath))
        {
            if (string.IsNullOrWhiteSpace(placesDbPath))
            {
                throw new ArgumentNullException(nameof(placesDbPath));
            }
        }

        public IPlaceInfo GetNearestPlace(double latitude, double longitude) => _tree.FindNearest(new PlaceInfo(latitude, longitude));

        public string GetNearestPlaceName(double latitude, double longitude) => _tree.FindNearest(new PlaceInfo(latitude, longitude))?.Name;
    }
}
