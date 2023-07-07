using System;
using System.Timers;

namespace BlazeWeather.Services;

public class PeriodicJobService : IDisposable
{
	public class PeriodicJobArgs : EventArgs { }
	public event EventHandler<PeriodicJobArgs>? Jobs;

	private Timer? timer;

	public void Start(TimeSpan interval)
	{
		timer?.Dispose();

		timer = new(interval.TotalMilliseconds);
		timer.Elapsed += HandleTimer;
		timer.AutoReset = true;
		timer.Enabled = true;
	}

	private void HandleTimer(object? source, ElapsedEventArgs e)
	{
		Jobs?.Invoke(source, new PeriodicJobArgs());
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		timer?.Dispose();
	}
}
