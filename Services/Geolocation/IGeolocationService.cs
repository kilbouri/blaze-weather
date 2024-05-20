using System.Threading.Tasks;
using BlazeWeather.Models.Domain;

namespace BlazeWeather.Services.Geolocation;

public interface IGeolocationService
{
    Task<Geocode?> GeolocateIpAddress(string? ipAddress);
}
