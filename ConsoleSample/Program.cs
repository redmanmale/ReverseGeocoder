using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSample
{
    class Program
    {
        static void Main(string[] args)
        {
            GeoSharp.ReverseGeoCode geocode = new GeoSharp.ReverseGeoCode(@"C:\\geodata\cities1000.txt", true);
            var name = geocode.NearestPlace(43.6442334, -79.3725049);
            Console.WriteLine(name.Name);
            Console.ReadLine();
        }
    }
}
