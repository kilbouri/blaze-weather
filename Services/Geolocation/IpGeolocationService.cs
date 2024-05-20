using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Models.IpGeolocation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazeWeather.Services.Geolocation;

public class IpGeolocationService : IGeolocationService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<IpGeolocationService> logger;
    private readonly string ipgeoApiKey;

    public IpGeolocationService(HttpClient httpClient, IOptions<AppSettings> opts, ILogger<IpGeolocationService> logger) {
        this.httpClient = httpClient;
        this.logger = logger;
        this.ipgeoApiKey = opts.Value.IpGeolocationApiKey;
    }

    public async Task<Geocode?> GeolocateIpAddress(string? ipAddress)
    {
        logger.LogInformation("Attmpting to geolocate {IpAddress}", ipAddress);

        var apiResponse = await FetchGeolocation(ipAddress);

        if (apiResponse == null) {
            logger.LogWarning("Failed to determine geocode for {IpAddress}", ipAddress);
            return null;
        }

        if (!double.TryParse(apiResponse.Latitude, out double lat))
        {
            logger.LogWarning("Failed to parse latitude '{LatitudeString}' from ipgeolocation.io response", apiResponse.Latitude);
            return null;
        }

        if (!double.TryParse(apiResponse.Longitude, out double lon)) {
            logger.LogWarning("Failed to parse longitude '{LongitudeString}' from ipgeolocation.io response", apiResponse.Longitude);
            return null;
        }

        return new Geocode() {
            Latitude = lat,
            Longitude = lon
        };
    }

    private async Task<IpGeolocationLookupResponse?> FetchGeolocation(string? ipAddress) {
        const string REQUIRED_FIELDS = "ip,latitude,longitude";

        try
        {
            string ipQuery = ipAddress != null ? $"ip={ipAddress}" : "";
            Uri apiUri = new UriBuilder()
            {
                Scheme = "https",
                Host = "api.ipgeolocation.io",
                Path = $"ipgeo",
                Query = $"apiKey={ipgeoApiKey}&fields={REQUIRED_FIELDS}&{ipQuery}",
            }.Uri;

            logger.LogInformation("Request URI: {Uri}", apiUri.AbsoluteUri);

            return await httpClient.GetFromJsonAsync<IpGeolocationLookupResponse>(apiUri);
        }
        catch (HttpRequestException exception)
        {
            logger.LogError(exception, "Failed to retrieve geolocation for {IpAddress}", ipAddress);
            return null;
        }
    }
}
