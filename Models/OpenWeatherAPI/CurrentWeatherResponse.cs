namespace BlazeWeather.Models.OpenWeatherAPI;

public class CurrentWeatherResponse : OpenWeatherAPIResponse
{
    public class MainWeatherPayload
    {
        public float Temp { get; set; }
        public float FeelsLike { get; set; }
        public float TempMin { get; set; }
        public float TempMax { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
    }

    public MainWeatherPayload Main { get; set; } = new();
}
