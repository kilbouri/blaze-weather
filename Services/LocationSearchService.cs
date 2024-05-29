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

// TomTom's API (used for geocoding & search completion) has a per-second
// and per-day rate limit so we use a fixed update interval while the user is typing
// to avoid rapid depletion of either cap.
public class LocationSearchService
{
    public event Action<GeocodeOptions>? ResultsUpdated;

    private readonly IGeocoderService geocoder;
    private readonly ILogger<LocationSearchService> logger;
    private readonly PeriodicJobService periodicJobService;

    private bool suspended = false;
    private string? lastQuery = null;
    private string? query = null;
    private Geocode? geobias = null;

    private CancellationTokenSource? mostRecentJobTokenSource = null;

    public LocationSearchService(IGeocoderService geocoder, PeriodicJobService periodicJobService, ILogger<LocationSearchService> logger)
    {
        this.geocoder = geocoder;
        this.periodicJobService = periodicJobService;
        this.logger = logger;

        periodicJobService.Jobs += PeriodicUpdateJob;
    }

    public void UpdateQuery(string newQuery)
    {
        query = newQuery;
        StartFetchJob();
    }

    public void UpdateGeobias(Geocode? newGeocode)
    {
        geobias = newGeocode;
        StartFetchJob();
    }

    public void Suspend()
    {
        suspended = true;
        periodicJobService.Stop();
    }

    public void Resume()
    {
        suspended = false;
        StartFetchJob();
    }

    private void PeriodicUpdateJob(PeriodicJobService.PeriodicJobArgs args)
    {
        bool queryChanged = lastQuery == null || lastQuery != query;
        lastQuery = query;

        if (!queryChanged || string.IsNullOrWhiteSpace(query))
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

        var newResults = await geocoder.GetCitySuggestions(query, limit: 50, geobias, mostRecentJobTokenSource.Token);

        // cancel the previous job if there is one, as its results are now outdated
        previousJobSource?.Cancel();

        if (mostRecentJobTokenSource.IsCancellationRequested)
        {
            // a subsequent job already completed, these results are outdated
            logger.LogWarning("Fetched {Count} results for '{Query}', but they are already outdated", newResults.Count(), query);
            return;
        }

        // Update results with the newly fetched results
        ResultsUpdated?.Invoke(newResults);
        logger.LogInformation("Fetched {Count} updated results for '{Query}'", newResults.Count(), query);
    }

    private void StartFetchJob() {
        if (!suspended)
        {
            periodicJobService.Start(TimeSpan.FromMilliseconds(250), !periodicJobService.IsRunning);
        }
    }
}
