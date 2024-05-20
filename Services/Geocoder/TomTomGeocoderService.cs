using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.TomTom;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services.Geocoders;

public class TomTomGeocoderService : IGeocoderService
{
    private readonly HttpClient httpClient;
    private readonly string tomtomApiKey;
    private readonly ILogger<TomTomGeocoderService> logger;

    public TomTomGeocoderService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<TomTomGeocoderService> logger)
    {
        this.httpClient = httpClient;
        this.tomtomApiKey = opts.Value.TomTomApiKey;
        this.logger = logger;
    }

    public async Task<IEnumerable<GeocodeOption>> GetCitySuggestions(string city, int limit = 5, Geocode? bias = null, CancellationToken token = default)
    {
        if (limit <= 0)
        {
            logger.LogWarning("Requested limit of {Limit} is less than 1", limit);
            return Enumerable.Empty<GeocodeOption>();
        }

        const int API_MAX_RESULTS = 2000;
        if (limit > API_MAX_RESULTS)
        {
            logger.LogWarning("Capping requested limit of {Limit} to {ApiLimit} due to API restriction", limit, API_MAX_RESULTS);
            limit = API_MAX_RESULTS;
        }

        GeocodingSearchResponse? searchResults = await GetDirectCityGeocoding(city, limit, bias, token);
        if (searchResults == null)
        {
            logger.LogWarning("Failed to retrieve geocode suggestions for {Location}", city);
            return Enumerable.Empty<GeocodeOption>();
        }

        logger.LogInformation("Retrieved {Count} geocode suggestions for {Location}", searchResults.Results.Count, city);

        IEnumerable<GeocodeOption> options = searchResults.Results
                            .Select(result => new GeocodeOption
                            {
                                City = result.Address.Municipality,
                                State = result.Address.CountrySubdivision,
                                Country = result.Address.Country,
                                Distance = result.Dist,
                                Confidence = result.Score,
                                ResolvedGeocode = new Geocode()
                                {
                                    Latitude = result.Position.Lat,
                                    Longitude = result.Position.Lon
                                }
                            });

        logger.LogInformation("Parsed {Count} geocode suggestions for {Location}", searchResults.Results.Count, city);
        return options;
    }

    private async Task<GeocodingSearchResponse?> GetDirectCityGeocoding(string location, int limit, Geocode? bias, CancellationToken token = default)
    {
        try
        {
            // Municipality = City, CountrySubdivision = State/Province/Territory/etc., Country = Country
            const string ENTITY_TYPE_SET = "Municipality";
            
            string geobias = bias != null ? $"lat={bias.Value.Latitude}&lon={bias.Value.Longitude}" : "";

            Uri apiUri = new UriBuilder()
            {
                Scheme = "https",
                Host = "api.tomtom.com",
                Path = $"search/2/search/{location}.json",
                Query = $"key={tomtomApiKey}&limit={limit}&entityTypeSet={ENTITY_TYPE_SET}&typeahead=true&{geobias}",
            }.Uri;

            logger.LogInformation("Request URI: {Uri}", apiUri.AbsoluteUri);

            return await httpClient.GetFromJsonAsync<GeocodingSearchResponse>(apiUri, token);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
