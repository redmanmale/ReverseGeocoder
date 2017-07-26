namespace ReverseGeocoding.Model
{
    public enum PlaceInfoFields
    {
        PlaceInfoId, //integer id of record in geonames database
        Name, //name of geographical point (utf8) varchar(200)
        AsciiName, //name of geographical point in plain ascii characters, varchar(200)
        AlternateNames, //alternatenames, comma separated, ascii names automatically transliterated, convenience attribute from alternatename table, varchar(8000)
        Latitude, //latitude in decimal degrees (wgs84)
        Longitude, //longitude in decimal degrees (wgs84)
        FeatureClass, //see http://www.geonames.org/export/codes.html, char(1)
        FeatureCode, //see http://www.geonames.org/export/codes.html, varchar(10)
        CountryCode, //ISO-3166 2-letter country code, 2 characters
        AltCountryCodes, //alternate country codes, comma separated, ISO-3166 2-letter country code, 60 characters
        Admin1Code, //fipscode (subject to change to iso code), see exceptions below, see file admin1Codes.txt for display names of this code; varchar(20)
        Admin2Code, //code for the second administrative division, a county in the US, see file admin2Codes.txt; varchar(80)
        Admin3Code, //code for third level administrative division, varchar(20)
        Admin4Code, //code for fourth level administrative division, varchar(20)
        Population, //bigint (8 byte int)
        Elevation, //in meters, integer
        Dem, //digital elevation model, srtm3 or gtopo30, average elevation of 3''x3'' (ca 90mx90m) or 30''x30'' (ca 900mx900m) area in meters, integer. srtm processed by cgiar/ciat.
        TimeZone, //the timezone id (see file timeZone.txt) varchar(40)
        ModificationDate, //date of last modification in yyyy-MM-dd format
    }
}
