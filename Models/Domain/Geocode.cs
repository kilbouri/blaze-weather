using System.ComponentModel.DataAnnotations;

namespace BlazeWeather.Models.Domain;

public struct Geocode
{
    public Geocode(double lat, double lng) : this()
    {
        Latitude = lat;
        Longitude = lng;
    }

    [Range(-180.0, 180.0)]
    public double Latitude { get; set; }

    [Range(-90.0, 90.0)]
    public double Longitude { get; set; }

    public readonly override string ToString()
    {
        return $"(Lat = {Latitude}, Long = {Longitude})";
    }
}
