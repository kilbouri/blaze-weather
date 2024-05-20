namespace BlazeWeather;

public class AppSettings
{
    /// <summary>
    /// API key for the OpenWeather API. Obtain a key here:
    /// https://openweathermap.org
    /// </summary>
    public string OpenWeatherApiKey { get; set; } = null!;

    /// <summary>
    /// API key for the TomTom API. Obtain a key here:
    /// https://developer.tomtom.com
    /// </summary>
    public string TomTomApiKey { get; set; } = null!;

    /// <summary>
    /// API key for the IP Geolocation API provided by
    /// https://ipgeolocation.io.
    /// <summary>
    public string IpGeolocationApiKey { get; set; } = null!;
}
