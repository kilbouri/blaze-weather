using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;

namespace BlazeWeather.Services.Geocoders;

public interface IGeocoderService {
    Task<IEnumerable<GeocodeOption>> GetCitySuggestions(string city, int limit = 5, CancellationToken token = default);
}
