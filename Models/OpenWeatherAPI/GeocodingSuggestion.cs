using System.Collections.Generic;

namespace BlazeWeather.Models.OpenWeatherAPI;

public class GeocodingSuggestion
{
	public class LocalNameDictionary : Dictionary<string, string>
	{
		public string FeatureName { get => this["feature_name"]; }
		public string Ascii { get => this["ascii"]; }
	}

	public float Lat { get; set; }
	public float Lon { get; set; }

	public string Name { get; set; } = "";
	public string Country { get; set; } = "";
	public string State { get; set; } = "";

	public LocalNameDictionary LocalNames { get; set; } = new();
}
