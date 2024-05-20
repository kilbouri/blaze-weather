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
            logger.LogWarning("Unable to fetch weather at at (Latitude: {Lat}, Longitude: {Lng})", lat, lng);
            return null;
        }

        logger.LogInformation("Fetched weather at at (Latitude: {Lat}, Longitude: {Lng})", lat, lng);
        return new Models.Domain.Weather
        {
            Temperature = new Temperature(TemperatureUnit.Celsius, apiResponse.Main.Temp),
            TemperatureMin = new Temperature(TemperatureUnit.Celsius, apiResponse.Main.TempMin),
            TemperatureMax = new Temperature(TemperatureUnit.Celsius, apiResponse.Main.TempMax),
            FeelsLike = new Temperature(TemperatureUnit.Celsius, apiResponse.Main.FeelsLike),
            PressureHPA = apiResponse.Main.Pressure,
            HumidityPerecent = apiResponse.Main.Humidity,
        };
    }

    private async Task<CurrentWeatherResponse?> FetchWeather(double lat, double lng)
    {
        try
        {
            string uri = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={openWeatherApiKey}";
            return await httpClient.GetFromJsonAsync<CurrentWeatherResponse>(uri);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
