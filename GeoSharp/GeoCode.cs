using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeoSharp
{
    internal class GeoCode
    {
        /* I'm torn between using OSM data, regional heirachical polygons (though this is more for the reverse lookups) or the geoname stuff
         * For now I'm going to just focus on parsing out the names to allow for non-strict structuring etc.
         * Later on I'll probably provide a bit of both depending on what resolutions/accuracies are required
         */
        private Dictionary<string, GeoName> Names;

        public GeoCode(string database, bool majorPlacesOnly)
        {
            using (FileStream fs = new FileStream(database, FileMode.Open))
            {
                Initialize(fs, majorPlacesOnly);
            }
        }

        public GeoCode(Stream Database, bool majorPlacesOnly)
        {
            Initialize(Database, majorPlacesOnly);
        }

        public List<GeoName> GetPossibleLocations(string location)
        {
            List<GeoName> Places = new List<GeoName>();

            return Places;
        }

        private void Initialize(Stream input, bool majorPlacesOnly)
        {
            List<GeoName> places = new List<GeoName>();
            using (StreamReader db = new StreamReader(input))
            {
                string Line;
                while (!db.EndOfStream && (Line = db.ReadLine()) != null)
                {
                    var place = new GeoName(Line);
                    if (!majorPlacesOnly || place.FeatureClass != GeoFeatureClass.City)
                        places.Add(place);
                }
            }

            Names = new Dictionary<string, GeoName>();
            foreach (var p in places)
            {
            }
        }
    }
}