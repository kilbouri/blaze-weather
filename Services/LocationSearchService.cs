using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazeWeather.Models.Domain;
using BlazeWeather.Services.Geocoders;
using Microsoft.Extensions.Logging;

namespace BlazeWeather.Services;

using GeocodeOptions = IEnumerable<GeocodeOption>;

public class LocationSearchService
{
    private readonly IGeocoder geocoder;
    private readonly ILogger<LocationSearchService> logger;
    private readonly PeriodicJobService periodicJobService;

    private string? lastQuery = null;
    private string? query = null;

    private CancellationTokenSource? mostRecentJobTokenSource = null;
    private GeocodeOptions? mostRecentResults = null;

    public event Action<GeocodeOptions>? ResultsUpdated;

    public LocationSearchService(IGeocoder geocoder, PeriodicJobService periodicJobService, ILogger<LocationSearchService> logger)
    {
        this.geocoder = geocoder;
        this.periodicJobService = periodicJobService;
        this.logger = logger;

        periodicJobService.Jobs += PeriodicUpdateJob;
    }

    public void UpdateQuery(string newQuery)
    {
        query = newQuery;
        periodicJobService.Start(TimeSpan.FromMilliseconds(250), !periodicJobService.IsRunning);
    }

    public GeocodeOptions GetResults()
    {
        if (mostRecentResults == null)
        {
            return Enumerable.Empty<GeocodeOption>();
        }

        return mostRecentResults.ToList();
    }

    private void PeriodicUpdateJob(PeriodicJobService.PeriodicJobArgs args)
    {
        bool queryChanged = lastQuery != null && lastQuery == query;
        lastQuery = query;

        if (queryChanged && !string.IsNullOrWhiteSpace(query))
        {
            // Either the query hasn't changed since last tick, or this tick we have no meaningful query.
            // We can safely stop the timer and resume it next time the query is updated.

            periodicJobService.Stop();
            return;
        }

        // Discard is used to suppress the warning that this call is never awaited
        _ = FetchUpdatedResults();
    }

    private async Task FetchUpdatedResults()
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return; // should never happen, but appeases the nullability checker
        }

        CancellationTokenSource? previousJobSource = mostRecentJobTokenSource;
        mostRecentJobTokenSource = new();

        var newResults = await geocoder.GetCitySuggestions(query, 50, mostRecentJobTokenSource.Token);

        // cancel the previous job if there is one, as its results are now outdated
        previousJobSource?.Cancel();

        if (mostRecentJobTokenSource.IsCancellationRequested)
        {
            // a subsequent job already completed, these results are outdated
            return;
        }

        // Update results with the newly fetched results
        mostRecentResults = newResults;
        ResultsUpdated?.Invoke(newResults);
    }
}
