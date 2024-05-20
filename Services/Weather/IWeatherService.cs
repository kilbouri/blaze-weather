using System.Threading.Tasks;

namespace BlazeWeather.Services.Weather;

public interface IWeatherService {
    Task<Models.Domain.Weather?> GetCurrentWeather(double lat, double lon);
}
