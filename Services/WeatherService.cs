using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.OpenWeatherAPI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services;

public class WeatherService
{
	private readonly HttpClient httpClient;
	private readonly string openWeatherApiKey;
	private readonly ILogger<WeatherService> logger;

	public WeatherService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<WeatherService> logger)
	{
		this.httpClient = httpClient;
		this.openWeatherApiKey = opts.Value.OpenWeatherApiKey;
		this.logger = logger;
	}

	public async Task<Weather?> GetCurrentWeather(float lat, float lng)
	{
		CurrentWeatherResponse? apiResponse = await FetchWeather(lat, lng);

		if (apiResponse == null)
		{
			logger.LogWarning("Unable to fetch weather at at (Latitude: {Lat}, Longitude: {Lng})", lat, lng);
			return null;
		}

		logger.LogInformation("Fetched weather at at (Latitude: {Lat}, Longitude: {Lng})", lat, lng);
		return new Weather
		{
			Temperature = apiResponse.Main.Temp,
			TemperatureMin = apiResponse.Main.TempMin,
			TemperatureMax = apiResponse.Main.TempMax,
			FeelsLike = apiResponse.Main.FeelsLike,
			PressureHPA = apiResponse.Main.Pressure,
			HumidityPerecent = apiResponse.Main.Humidity,
		};
	}

	private async Task<CurrentWeatherResponse?> FetchWeather(float lat, float lng)
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
