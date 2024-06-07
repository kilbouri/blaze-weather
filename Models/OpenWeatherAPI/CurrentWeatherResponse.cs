using System.Text.Json.Serialization;

namespace BlazeWeather.Models.OpenWeatherAPI;

// https://openweathermap.org/current
public class CurrentWeatherResponse : OpenWeatherAPIResponse
{
    public class MainWeatherPayload
    {
        public float Temp { get; set; }
        [JsonPropertyName("feels_like")]
        public float FeelsLike { get; set; }
        public float TempMin { get; set; }
        public float TempMax { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
    }
    
    public class Coordinate
    {
        public float Lat { get; set; }
        public float Lon { get; set; }
    }

    public MainWeatherPayload Main { get; set; } = new();
    public Coordinate Coord { get; set; } = new();
}
