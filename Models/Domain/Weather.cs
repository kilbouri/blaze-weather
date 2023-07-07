namespace BlazeWeather.Models.Domain;

public struct Weather
{
	// NOTE: All temperatures in Celsius. I should document that...
	public float Temperature { get; set; }
	public float FeelsLike { get; set; }
	public float TemperatureMin { get; set; }
	public float TemperatureMax { get; set; }
	public float HumidityPerecent { get; set; }
	public float PressureHPA { get; set; }
}
