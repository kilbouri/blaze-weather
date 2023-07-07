using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.OpenWeatherAPI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services;

public class GeocodingService
{

	private readonly HttpClient httpClient;
	private readonly string openWeatherApiKey;
	private readonly ILogger<GeocodingService> logger;

	public GeocodingService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<GeocodingService> logger)
	{
		this.httpClient = httpClient;
		this.openWeatherApiKey = opts.Value.OpenWeatherAPIKey;
		this.logger = logger;
	}

	public async Task<IEnumerable<GeocodeOption>?> GetSuggestions(string city, int limit = 5)
	{
		List<GeocodingSuggestion>? apiResponse = await GetDirectGeocoding(city, limit);

		if (apiResponse == null)
		{
			logger.LogWarning("Failed to retrieve Geocode suggestions for {City}", city);
			return null;
		}

		return apiResponse.Select(result => new GeocodeOption()
		{
			City = result.Name,
			State = result.State,
			Country = result.Country,
			ResolvedGeocode = new Geocode
			{
				Latitude = result.Lat,
				Longitude = result.Lon
			}
		});
	}

	public async Task<Geocode?> GetFromCity(string city)
	{
		List<GeocodingSuggestion>? apiResponse = await GetDirectGeocoding(city, 1);

		if (apiResponse == null || !apiResponse.Any())
		{
			logger.LogWarning("Failed to retrieve Geocode for {City}", city);
			return null;
		}

		// TODO: can we somehow provide this decision to the user?
		GeocodingSuggestion acceptedSuggestion = apiResponse.First();
		logger.LogInformation("Determined {City} is located at {Geocode}", city, acceptedSuggestion);

		return new Geocode
		{
			Latitude = acceptedSuggestion.Lat,
			Longitude = acceptedSuggestion.Lon,
		};
	}

	private async Task<List<GeocodingSuggestion>?> GetDirectGeocoding(string cityDescription, int limit)
	{
		try
		{
			string urlSafeCity = Uri.EscapeDataString(cityDescription);
			string uri = $"http://api.openweathermap.org/geo/1.0/direct?q={urlSafeCity}&appid={openWeatherApiKey}&limit={limit}";
			return await httpClient.GetFromJsonAsync<List<GeocodingSuggestion>>(uri);
		}
		catch (HttpRequestException)
		{
			return null;
		}
	}
}
