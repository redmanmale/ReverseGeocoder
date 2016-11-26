using GeoSharp.KDTree;
using System.Collections.Generic;
using System.IO;

//All the geocoding data can be gotten from: http://download.geonames.org/export/dump/
namespace GeoSharp
{
    public class ReverseGeoCode
    {
        private KDTree<GeoName> Tree;

        public ReverseGeoCode(GeoDB database)
        {
            Tree = new KDTree<GeoName>(database.Places.ToArray());
        }

        public ReverseGeoCode(Stream placesDatabase, Stream countryInfoDatabase = null)
        {
            GeoDB geodb = new GeoSharp.GeoDB();
            if (countryInfoDatabase != null) geodb.SetCountryInfos(countryInfoDatabase);
            geodb.SetPlaces(placesDatabase);

            Tree = new KDTree<GeoName>(geodb.Places.ToArray());
        }

        public ReverseGeoCode(string placesDatabase, string countryInfoDatabase = null)
        {
            GeoDB geodb = new GeoSharp.GeoDB();
            if (countryInfoDatabase != null) geodb.SetCountryInfos(countryInfoDatabase);
            geodb.SetPlaces(placesDatabase);

            Tree = new KDTree<GeoName>(geodb.Places.ToArray());
        }

        public GeoName NearestPlace(double latitude, double longitude)
        {
            return Tree.FindNearest(new GeoName(latitude, longitude));
        }

        public string NearestPlaceName(double latitude, double longitude)
        {
            return Tree.FindNearest(new GeoName(latitude, longitude)).Name;
        }
        
    }
}