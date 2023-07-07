using System.Text.Json.Serialization;

namespace BlazeWeather.Models.OpenWeatherAPI;

public class OpenWeatherAPIResponse
{
	[JsonPropertyName("cod")]
	public int Code { get; set; } = -1;

	[JsonPropertyName("")]
	public string Message { get; set; } = "";
}
