using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            GeoSharp.ReverseGeoCode geocode = new GeoSharp.ReverseGeoCode(@"C:\\geodata\cities5000.txt");
            var place = geocode.NearestPlace(48.1158333, 16.5643811);
            Console.WriteLine("Name: " + place.Name);
            Console.WriteLine("Feature Class: " + place.FeatureClass);
            Console.WriteLine("Country Code: " + place.CountryCode);

            Console.ReadLine();
        }
    }
}