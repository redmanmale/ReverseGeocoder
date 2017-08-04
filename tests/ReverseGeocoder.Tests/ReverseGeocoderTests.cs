using System;
using System.IO;
using NUnit.Framework;
using ReverseGeocoding.Interface;

namespace ReverseGeocoding.Tests
{
    [TestFixture]
    public class ReverseGeocoderTests
    {
        private const string citiesDb = "cities1000.txt";
        private const string countresDb = "countryInfo.txt";

        private static string CitiesDbPath => Path.Combine(TestContext.CurrentContext.TestDirectory, citiesDb);

        private static string CountresDbPath => Path.Combine(TestContext.CurrentContext.TestDirectory, countresDb);

        [TestCase(-6.48449, -46.07212, "Grajaú")]
        [TestCase(54.30208, 136.45948, "Chumikan")]
        [TestCase(21.92051, 99.15254, "Nayun")]
        public void PlacePathTest(double latitude, double longitude, string placeName)
        {
            IReverseGeocoder geocoder = new ReverseGeocoder(CitiesDbPath);
            var place = geocoder.GetNearestPlace(latitude, longitude);

            Assert.NotNull(place);
            Assert.AreEqual(placeName, place.Name);
        }

        [TestCase(-6.48449, -46.07212, "Grajaú", "Brazil")]
        [TestCase(54.30208, 136.45948, "Chumikan", "Russia")]
        [TestCase(21.92051, 99.15254, "Nayun", "China")]
        public void CountryPathTest(double latitude, double longitude, string placeName, string countryName)
        {
            IReverseGeocoder geocoder = new ReverseGeocoder(CitiesDbPath, CountresDbPath);
            var place = geocoder.GetNearestPlace(latitude, longitude);

            Assert.NotNull(place);
            Assert.AreEqual(placeName, place.Name);
            Assert.NotNull(place.CountryInfo);
            Assert.AreEqual(countryName, place.CountryInfo.Country);
        }

        [TestCase(-6.48449, -46.07212, "Grajaú")]
        [TestCase(54.30208, 136.45948, "Chumikan")]
        [TestCase(21.92051, 99.15254, "Nayun")]
        public void PlaceStreamTest(double latitude, double longitude, string placeName)
        {
            using (var fileStream = File.OpenRead(CitiesDbPath))
            {
                IReverseGeocoder geocoder = new ReverseGeocoder(fileStream);
                var place = geocoder.GetNearestPlace(latitude, longitude);

                Assert.NotNull(place);
                Assert.AreEqual(placeName, place.Name);
            }
        }

        [TestCase(-6.48449, -46.07212, "Grajaú", "Brazil")]
        [TestCase(54.30208, 136.45948, "Chumikan", "Russia")]
        [TestCase(21.92051, 99.15254, "Nayun", "China")]
        public void CountryStreamTest(double latitude, double longitude, string placeName, string countryName)
        {
            using (var fileStreamCities = File.OpenRead(CitiesDbPath))
            using (var fileStreamCountries = File.OpenRead(CountresDbPath))
            {
                IReverseGeocoder geocoder = new ReverseGeocoder(fileStreamCities, fileStreamCountries);
                var place = geocoder.GetNearestPlace(latitude, longitude);

                Assert.NotNull(place);
                Assert.AreEqual(placeName, place.Name);
                Assert.NotNull(place.CountryInfo);
                Assert.AreEqual(countryName, place.CountryInfo.Country);
            }
        }

        [TestCase(-6.48449, -46.07212, "Grajaú")]
        [TestCase(54.30208, 136.45948, "Chumikan")]
        [TestCase(21.92051, 99.15254, "Nayun")]
        public void PlaceDbTest(double latitude, double longitude, string placeName)
        {
            var geoDb = new GeoDb(CitiesDbPath);

            IReverseGeocoder geocoder = new ReverseGeocoder(geoDb);
            var place = geocoder.GetNearestPlace(latitude, longitude);

            Assert.NotNull(place);
            Assert.AreEqual(placeName, place.Name);
        }

        [TestCase(-6.48449, -46.07212, "Grajaú", "Brazil")]
        [TestCase(54.30208, 136.45948, "Chumikan", "Russia")]
        [TestCase(21.92051, 99.15254, "Nayun", "China")]
        public void CountryDbTest(double latitude, double longitude, string placeName, string countryName)
        {
            var geoDb = new GeoDb(CitiesDbPath, CountresDbPath);

            IReverseGeocoder geocoder = new ReverseGeocoder(geoDb);
            var place = geocoder.GetNearestPlace(latitude, longitude);

            Assert.NotNull(place);
            Assert.AreEqual(placeName, place.Name);
            Assert.NotNull(place.CountryInfo);
            Assert.AreEqual(countryName, place.CountryInfo.Country);
        }

        [TestCase(-6.48449, -46.07212, "Grajaú")]
        [TestCase(54.30208, 136.45948, "Chumikan")]
        [TestCase(21.92051, 99.15254, "Nayun")]
        public void PlaceNamePathTest(double latitude, double longitude, string placeNameReference)
        {
            IReverseGeocoder geocoder = new ReverseGeocoder(CitiesDbPath);
            var placeName = geocoder.GetNearestPlaceName(latitude, longitude);

            Assert.AreEqual(placeNameReference, placeName);
        }

        /// <summary>
        /// Latitudes range from -90 to 90.
        /// Longitudes range from -180 to 180.
        /// </summary>
        [TestCase(-90, -180)]
        [TestCase(-90, 180)]
        [TestCase(90, -180)]
        [TestCase(90, 180)]
        [TestCase(0, 0)]
        [TestCase(-91, -181)]
        [TestCase(-91, 181)]
        [TestCase(91, -181)]
        [TestCase(91, 181)]
        [TestCase(200, 200)]
        [TestCase(double.MaxValue, double.MaxValue)]
        [TestCase(double.MinValue, double.MaxValue)]
        [TestCase(double.MaxValue, -double.MinValue)]
        [TestCase(double.MinValue, -double.MinValue)]
        [TestCase(double.NaN, double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity, double.PositiveInfinity)]
        [TestCase(double.PositiveInfinity, double.NaN)]
        public void OddCoordinatesTest(double latitude, double longitude)
        {
            IReverseGeocoder geocoder = new ReverseGeocoder(CitiesDbPath);
            Assert.DoesNotThrow(() => geocoder.GetNearestPlace(latitude, longitude));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" \t    \t")]
        public void CityPathNullTest(string parameter)
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ReverseGeocoder(parameter));
        }

        [Test]
        public void CityStreamNullTest()
        {
            Stream parameter = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new ReverseGeocoder(parameter));
        }
    }
}
