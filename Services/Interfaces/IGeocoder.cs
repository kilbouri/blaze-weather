using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;

namespace BlazeWeather.Services.Interfaces;

public interface IGeocoder {
    Task<IEnumerable<GeocodeOption>> GetCitySuggestions(string city, int limit = 5, CancellationToken token = default);
}
