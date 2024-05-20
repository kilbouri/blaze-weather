using System;
using System.Timers;

namespace BlazeWeather.Services;

public class PeriodicJobService
{
    public class PeriodicJobArgs : EventArgs
    {
        public int InvokeId { get; }
        public PeriodicJobArgs(int invokeId) => this.InvokeId = invokeId;
    }
    
    public event Action<PeriodicJobArgs>? Jobs;

    private readonly Timer timer = new();
    private int internalInvokeId = 0;

    public bool IsRunning => timer.Enabled;

    public void Start(TimeSpan interval, bool triggerImmediately = false)
    {
        timer.Stop();

        timer.Interval = interval.TotalMilliseconds;
        timer.Elapsed += (_, _) => HandleTimer();
        timer.AutoReset = true;

        if (triggerImmediately)
        {
            HandleTimer();
        }

        timer.Start();
    }

    public void Stop()
    {
        timer.Stop();
    }

    private void HandleTimer()
    {
        PeriodicJobArgs invokeArgs = new(internalInvokeId++);
        Jobs?.Invoke(invokeArgs);
    }
}
