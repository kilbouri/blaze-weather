using System;
using System.Timers;

namespace BlazeWeather.Services;

public class PeriodicJobService : IDisposable
{
	public class PeriodicJobArgs : EventArgs
	{
		public int InvokeId { get; }

		public PeriodicJobArgs(int invokeId) => this.InvokeId = invokeId;
	}

	public event Action<PeriodicJobArgs>? Jobs;
	private Timer? timer;
	private int internalInvokeId = 0;

	public void Start(TimeSpan interval, bool triggerImmediately = false)
	{
		timer?.Dispose();

		timer = new(interval.TotalMilliseconds);
		timer.Elapsed += (_, _) => HandleTimer();
		timer.AutoReset = true;
		timer.Enabled = true;

		if (triggerImmediately)
		{
			HandleTimer();
		}
	}

	public void Stop(bool clearJobs = false)
	{
		timer?.Dispose();

		if (clearJobs)
		{
			Jobs = null;
		}
	}

	private void HandleTimer()
	{
		PeriodicJobArgs invokeArgs = new(internalInvokeId++);
		Jobs?.Invoke(invokeArgs);
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		timer?.Dispose();
	}
}
