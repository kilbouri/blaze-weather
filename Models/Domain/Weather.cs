namespace BlazeWeather.Models.Domain;

public struct Weather
{
    public Temperature Temperature { get; set; }
    public Temperature FeelsLike { get; set; }
    public Temperature TemperatureMin { get; set; }
    public Temperature TemperatureMax { get; set; }
    public float HumidityPerecent { get; set; }
    public float PressureHPA { get; set; }
}
