using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.TomTom;
using BlazeWeather.Services.ReverseGeocoders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services.Geocoders;

public class TomTomReverseGeocoderService : IReverseGeocoderService
{
    private readonly HttpClient httpClient;
    private readonly string tomtomApiKey;
    private readonly ILogger<TomTomGeocoderService> logger;

    public TomTomReverseGeocoderService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<TomTomGeocoderService> logger)
    {
        this.httpClient = httpClient;
        this.tomtomApiKey = opts.Value.TomTomApiKey;
        this.logger = logger;
    }

    public async Task<IEnumerable<GeocodeOption>> GetGeocodeOptions(Geocode coordinates, CancellationToken token = default)
    {
        ReverseGeocodingSearchResponse? searchResults = await GetGeocodeOptionsInternal(coordinates.Latitude, coordinates.Longitude, token);
        if (searchResults == null)
        {
            logger.LogWarning("Failed to reverse geocode {Location}", coordinates);
            return Enumerable.Empty<GeocodeOption>();
        }

        logger.LogInformation("Retrieved {Count} geocode suggestions for {Location}", searchResults.Summary.NumResults, coordinates);

        var options = searchResults.Addresses
                                .Select(addr => new GeocodeOption
                                {
                                    City = addr.Address.Municipality,
                                    State = addr.Address.CountrySubdivision,
                                    Country = addr.Address.Country,
                                    Geocode = new Geocode()
                                    {
                                        Latitude = double.Parse(addr.Position.Split(',', 2)[0]),
                                        Longitude = double.Parse(addr.Position.Split(',', 2)[1]),
                                    }
                                });

        logger.LogInformation("Parsed {Count} geocode suggestions for {Location}", options.Count(), coordinates);
        return options;
    }

    private async Task<ReverseGeocodingSearchResponse?> GetGeocodeOptionsInternal(double lat, double lon, CancellationToken token)
    {
        // Municipality = City, CountrySubdivision = State/Province/Territory/etc., Country = Country
        const string ENTITY_TYPE = "Municipality";

        try
        {
            Uri apiUri = new UriBuilder()
            {
                Scheme = "https",
                Host = "api.tomtom.com",
                Path = $"search/2/reverseGeocode/{lat},{lon}.json",
                Query = $"key={tomtomApiKey}&entityType={ENTITY_TYPE}",
            }.Uri;

            logger.LogInformation("Request URI: {Uri}", apiUri.AbsoluteUri);

            return await httpClient.GetFromJsonAsync<ReverseGeocodingSearchResponse>(apiUri, token);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
