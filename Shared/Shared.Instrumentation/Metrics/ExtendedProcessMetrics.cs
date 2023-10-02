using System.Diagnostics.Metrics;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace Shared.Instrumentation.Metrics
{
	internal sealed class ExtendedProcessMetrics
	{
		internal static readonly AssemblyName AssemblyName = typeof(ExtendedProcessMetrics).Assembly.GetName();
		internal static readonly string MeterName = AssemblyName.Name;
		private static readonly Meter MeterInstance = new(MeterName, AssemblyName.Version.ToString());

		// For process.cpu.utilization
		private static ThreadLocal<double> previousUserTime = new(() => 0);
		private static ThreadLocal<long> previousUserTimestampTicks = new(() => DateTime.UtcNow.Ticks);

		// For process.cpu.utilization.system
		private static ThreadLocal<double> previousSystemTime = new(() => 0);
		private static ThreadLocal<long> previousSystemTimestampTicks = new(() => DateTime.UtcNow.Ticks);

		// For process.cpu.utilization.total
		private static ThreadLocal<double> previousOverallTotalTime = new(() => 0);
		private static ThreadLocal<long> previousOverallTimestampTicks = new(() => DateTime.UtcNow.Ticks);

		static ExtendedProcessMetrics()
		{
			MeterInstance.CreateObservableGauge(
				"process.cpu.utilization",
				() =>
				{
					var process = Process.GetCurrentProcess();
					var userTime = process.UserProcessorTime.TotalSeconds;
					var currentTimestampTicks = DateTime.UtcNow.Ticks;

					var prevUserTime = previousUserTime.Value;
					previousUserTime.Value = userTime;

					var prevTimestampTicks = previousUserTimestampTicks.Value;
					previousUserTimestampTicks.Value = currentTimestampTicks;

					var deltaUser = userTime - prevUserTime;
					double elapsedWallClockTime = new TimeSpan(currentTimestampTicks - prevTimestampTicks).TotalSeconds;

					return (elapsedWallClockTime <= 0) ? 0 : (deltaUser / elapsedWallClockTime) * 100.0;
				},
				unit: "%",
				description: "The CPU utilization in percentage based on user time.");

			MeterInstance.CreateObservableGauge(
				"process.cpu.utilization.system",
				() =>
				{
					var process = Process.GetCurrentProcess();
					var systemTime = process.PrivilegedProcessorTime.TotalSeconds;
					var currentTimestampTicks = DateTime.UtcNow.Ticks;

					var prevSystemTime = previousSystemTime.Value;
					previousSystemTime.Value = systemTime;

					var prevTimestampTicks = previousSystemTimestampTicks.Value;
					previousSystemTimestampTicks.Value = currentTimestampTicks;

					var deltaSystem = systemTime - prevSystemTime;
					double elapsedWallClockTime = new TimeSpan(currentTimestampTicks - prevTimestampTicks).TotalSeconds;

					return (elapsedWallClockTime <= 0) ? 0 : (deltaSystem / elapsedWallClockTime) * 100.0;
				},
				unit: "%",
				description: "The CPU utilization in percentage based on system time.");

			MeterInstance.CreateObservableGauge(
				"process.cpu.utilization.total",
				() =>
				{
					var process = Process.GetCurrentProcess();
					var currentTotalTime = process.TotalProcessorTime.TotalSeconds;
					var currentTimestampTicks = DateTime.UtcNow.Ticks;

					var prevTotalTime = previousOverallTotalTime.Value;
					previousOverallTotalTime.Value = currentTotalTime;

					var prevTimestampTicks = previousOverallTimestampTicks.Value;
					previousOverallTimestampTicks.Value = currentTimestampTicks;

					double deltaTotal = currentTotalTime - prevTotalTime;
					double elapsedWallClockTime = new TimeSpan(currentTimestampTicks - prevTimestampTicks).TotalSeconds;

					return (elapsedWallClockTime <= 0) ? 0 : (deltaTotal / elapsedWallClockTime) * 100.0;
				},
				unit: "%",
				description: "Overall CPU utilization in percentage.");
		}

		public ExtendedProcessMetrics() { }
	}
}
