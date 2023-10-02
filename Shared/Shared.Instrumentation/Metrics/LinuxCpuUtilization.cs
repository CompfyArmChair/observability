namespace Shared.Instrumentation.Metrics;

public class LinuxCpuUtilization
{
	private long prevTotalTime = 0;
	private long prevActiveTime = 0;

	public double GetCpuUtilization()
	{
		try
		{
			var statsLine = File.ReadLines("/proc/stat").First();
			var statsParts = statsLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			var userTime = long.Parse(statsParts[1]);
			var niceTime = long.Parse(statsParts[2]);
			var systemTime = long.Parse(statsParts[3]);
			var idleTime = long.Parse(statsParts[4]);
			var iowaitTime = long.Parse(statsParts[5]);
			var irqTime = long.Parse(statsParts[6]);
			var softirqTime = long.Parse(statsParts[7]);
			var stealTime = long.Parse(statsParts[8]);
			var guestTime = long.Parse(statsParts[9]);

			long activeTime = userTime + niceTime + systemTime + irqTime + softirqTime + stealTime + guestTime;
			long totalTime = activeTime + idleTime + iowaitTime;

			var previousActive = Interlocked.Exchange(ref prevActiveTime, activeTime);
			var previousTotal = Interlocked.Exchange(ref prevTotalTime, totalTime);

			var diffActive = activeTime - previousActive;
			var diffTotal = totalTime - previousTotal;

			if (diffTotal <= 0) return 0;

			return 100.0 * diffActive / diffTotal;
		}
		catch (Exception ex)  // Prefer specific exceptions
		{
			// Log the error with a logger of your choice
			// Console.WriteLine(ex);
			return -1;
		}
	}
}
