using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BlazeWeather.Models.OpenWeatherAPI;

// https://openweathermap.org/current
public class CurrentWeatherResponse : OpenWeatherAPIResponse
{
    public class CoordinatePayload
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class WeatherDetailsPayload
    {
        public int Id { get; set; }
        public string Main { get; set; } = "";
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";
    }
    
    public class MainPayload
    {
        public double Temp { get; set; }
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }
        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }
        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        public int Humidity { get; set; }

        public int Pressure { get; set; }
        [JsonPropertyName("sea_level")]
        public int SeaLevel { get; set; }
        [JsonPropertyName("grnd_level")]
        public int GrndLevel { get; set; }
    }

    public class WindPayload
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double Gust { get; set; }
    }
    
    public class CloudsPayload
    {
        public int All { get; set; }
    }

    public class RainPayload
    {
        [JsonPropertyName("1h")]
        public double OneHour { get; set; }
        [JsonPropertyName("3h")]
        public double ThreeHour { get; set; }
    }
    
    public class SnowPayload
    {
        [JsonPropertyName("1h")]
        public double OneHour { get; set; }
        [JsonPropertyName("3h")]
        public double ThreeHour { get; set; }
    }

    public class SysPayload
    {
        public long Sunrise { get; set; }
        public long Sunset { get; set; }
    }

    public CoordinatePayload Coord { get; set; } = new();
    public WeatherDetailsPayload[] Weather { get; set; } = Array.Empty<WeatherDetailsPayload>();
    public MainPayload Main { get; set; } = new();
    public int Visibility { get; set; }
    public WindPayload Wind { get; set; } = new();
    public CloudsPayload Clouds { get; set; } = new();
    public RainPayload? Rain { get; set; } = null;
    public SnowPayload? Snow { get; set; } = null;
    public long Dt { get; set; }
    public SysPayload Sys { get; set; } = new();
}
