using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.OpenWeatherAPI;
using BlazeWeather.Services.Weather;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services;

public class OpenWeatherAPIWeatherService : IWeatherService
{
    private readonly HttpClient httpClient;
    private readonly string openWeatherApiKey;
    private readonly ILogger<OpenWeatherAPIWeatherService> logger;

    public OpenWeatherAPIWeatherService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<OpenWeatherAPIWeatherService> logger)
    {
        this.httpClient = httpClient;
        this.openWeatherApiKey = opts.Value.OpenWeatherApiKey;
        this.logger = logger;
    }

    public async Task<Models.Domain.Weather?> GetCurrentWeather(double lat, double lng)
    {
        CurrentWeatherResponse? apiResponse = await FetchWeather(lat, lng);

        if (apiResponse == null)
        {
            logger.LogWarning("Unable to fetch weather at (Latitude: {Lat}, Longitude: {Lng})", lat, lng);
            return null;
        }

        logger.LogInformation("Fetched weather at (Latitude: {Lat}, Longitude: {Lng})", lat, lng);
        return new Models.Domain.Weather
        {
            Location = new Geocode(apiResponse.Coord.Lat, apiResponse.Coord.Lon),

            CurrentTemp = new Temperature(TemperatureUnit.Kelvin, apiResponse.Main.Temp),
            FeelsLike = new Temperature(TemperatureUnit.Kelvin, apiResponse.Main.FeelsLike),
            MinTemp = new Temperature(TemperatureUnit.Kelvin, apiResponse.Main.TempMin),
            MaxTemp = new Temperature(TemperatureUnit.Kelvin, apiResponse.Main.TempMax),
            
            HumidityPerecent = apiResponse.Main.Humidity,

            GroundPressureHPA = apiResponse.Main.GrndLevel,
            SeaPressureHPA = apiResponse.Main.SeaLevel,

            RainMillimeters = apiResponse.Rain?.OneHour ?? 0.0,
            SnowMillimeters = apiResponse.Snow?.OneHour ?? 0.0,

            WindSpeed = apiResponse.Wind.Speed,
            WindGust = apiResponse.Wind.Gust,
            WindDegrees = apiResponse.Wind.Deg,

            CloudsPercent = apiResponse.Clouds.All,

            Sunrise = TimeOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(apiResponse.Sys.Sunrise).DateTime),
            Sunset = TimeOnly.FromDateTime(DateTimeOffset.FromUnixTimeSeconds(apiResponse.Sys.Sunset).DateTime),

            IconUrl = GetPrimaryIconUrl(apiResponse)
        };
    }

    private async Task<CurrentWeatherResponse?> FetchWeather(double lat, double lng)
    {
        try
        {
            Uri apiUri = new UriBuilder()
            {
                Scheme = "https",
                Host = "api.openweathermap.org",
                Path = $"data/2.5/weather",
                Query = $"appid={openWeatherApiKey}&lat={lat}&lon={lng}",
            }.Uri;
            
            logger.LogInformation("Request URI: {Uri}", apiUri.AbsoluteUri);
            return await httpClient.GetFromJsonAsync<CurrentWeatherResponse>(apiUri);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    private static string GetPrimaryIconUrl(CurrentWeatherResponse response)
    {
        if (response.Weather.Length <= 0)
        {
            return "";
        }

        CurrentWeatherResponse.WeatherDetailsPayload primary = response.Weather[0];
        return $"https://openweathermap.org/img/wn/{primary.Icon}@4x.png";
    }
}
