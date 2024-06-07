using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;

namespace BlazeWeather.Services.ReverseGeocoders;

public interface IReverseGeocoderService
{
    public Task<IEnumerable<GeocodeOption>> GetGeocodeOptions(Geocode coordinates, CancellationToken token = default);
}
