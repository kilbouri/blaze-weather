using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.TomTom;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services;

public class GeocodingService
{

	private readonly HttpClient httpClient;
	private readonly string tomtomApiKey;
	private readonly ILogger<GeocodingService> logger;

	public GeocodingService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<GeocodingService> logger)
	{
		this.httpClient = httpClient;
		this.tomtomApiKey = opts.Value.TomTomApiKey;
		this.logger = logger;
	}

	public async Task<IEnumerable<GeocodeOption>> GetCitySuggestions(string city, int limit = 5)
	{
		if (limit <= 0)
		{
			logger.LogWarning("Requested limit of {Limit} is less than 1", limit);
			return Enumerable.Empty<GeocodeOption>();
		}

		const int RESULTS_LIMIT = 2000;
		if (limit > RESULTS_LIMIT)
		{
			logger.LogWarning("Capping requested limit of {Limit} to {ApiLimit} due to API restriction", limit, RESULTS_LIMIT);
			limit = RESULTS_LIMIT;
		}

		GeocodingSearchResult? searchResults = await GetDirectCityGeocoding(city, limit);
		if (searchResults == null)
		{
			logger.LogWarning("Failed to retrieve geocode suggestions for {Location}", city);
			return Enumerable.Empty<GeocodeOption>();
		}

		logger.LogInformation("Retrieved {Count} geocode suggestions for {Location}", searchResults.Results.Count, city);

		return searchResults.Results
							.OrderByDescending(result => result.MatchConfidence.Score)
							.Select(result => new GeocodeOption
							{
								City = result.Address.Municipality,
								State = result.Address.CountrySubdivision,
								Country = result.Address.Country,
								Confidence = result.MatchConfidence.Score,
								ResolvedGeocode = new Geocode()
								{
									Latitude = result.Position.Lat,
									Longitude = result.Position.Lon
								}
							});
	}

	private async Task<GeocodingSearchResult?> GetDirectCityGeocoding(string location, int limit)
	{
		try
		{
			// Municipality = City, CountrySubdivision = State/Province/Territory/etc., Country = Country
			const string ENTITY_TYPE_SET = "Municipality";
			Uri apiUri = new UriBuilder()
			{
				Scheme = "https",
				Host = "api.tomtom.com",
				Path = $"search/2/geocode/{location}.json",
				Query = $"key={tomtomApiKey}&limit={limit}&entityTypeSet={ENTITY_TYPE_SET}",
			}.Uri;

			logger.LogInformation("Request URI: {Uri}", apiUri.AbsoluteUri);

			return await httpClient.GetFromJsonAsync<GeocodingSearchResult>(apiUri);
		}
		catch (HttpRequestException)
		{
			return null;
		}
	}
}
