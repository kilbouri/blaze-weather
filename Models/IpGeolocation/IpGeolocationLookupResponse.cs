namespace BlazeWeather.Models.IpGeolocation;

public class IpGeolocationLookupResponse
{
    public string Ip {get;set;} = null!;
    public string Latitude {get;set;} = null!;
    public string Longitude {get;set;} = null!;
}
