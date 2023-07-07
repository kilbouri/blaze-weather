using System.ComponentModel.DataAnnotations;

namespace BlazeWeather.Models.Domain;

public struct Geocode
{
	[Range(-180.0, 180.0)]
	public float Latitude { get; set; }

	[Range(-90.0, 90.0)]
	public float Longitude { get; set; }

	public readonly override string ToString()
	{
		return $"(Lat = {Latitude}, Long = {Longitude})";
	}
}
