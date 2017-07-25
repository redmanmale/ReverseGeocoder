# ReverseGeocoder

[![Build status](https://ci.appveyor.com/api/projects/status/q7v8w5qt73b8gd38?svg=true)](https://ci.appveyor.com/project/redmanmale/reversegeocoder)

An offline reverse geocoding library in C#. As a source of geodata may be used [GeoNames.org](http://geonames.org/).

## Getting started

### Get geodata

For this library to work offline you need to download in advance the geodata files (from [here](http://download.geonames.org/export/dump/)). You can get file for one country or with all cities of the world with population more than 15k, 5k or 1k.

Also you may need to get country list to be able to get country name by its code.

### Basic usage

There are two options:

* Load a geodata from a file
```cs
var geocoder = new ReverseGeocoder(@"C:\cities1000.txt");
Console.WriteLine(geocoder.NearestPlaceName(40.730885, -73.997383));
```

* Load a geodata from a `Stream`
```cs
using (MemoryStream ms = new MemoryStream(geodata))
{
	GeoSharp.ReverseGeoCoder geocoder = new GeoSharp.ReverseGeoCoder(ms);
	Console.WriteLine(geocoder.NearestPlaceName(40.730885, -73.997383));
}
```

### Perfomance

The look ups are very fast, `O(logn)`, however building the initial KD-Tree is not so fast, `O(kn * logn)`, so pre-selecting your datasets and filtering out places or areas that are not of interest (like water bodies) can be used to improve load speed and reduce memory usage.

## Acknowledgment

Based on [GeoSharp](https://github.com/gdegeneve/GeoSharp) by [gdegeneve](https://github.com/gdegeneve).
