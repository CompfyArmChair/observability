using System.Diagnostics.Metrics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Shared.Instrumentation.Metrics;
internal sealed class ExtendedSystemMetrics
{
	internal static readonly AssemblyName AssemblyName = typeof(ExtendedProcessMetrics).Assembly.GetName();
	internal static readonly string MeterName = AssemblyName.Name;
	private static readonly Meter MeterInstance = new(MeterName, AssemblyName.Version.ToString());

	private static readonly LinuxCpuUtilization linuxCpuUtilization = new LinuxCpuUtilization();

	static ExtendedSystemMetrics()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			MeterInstance.CreateObservableGauge(
				"system.cpu.utilization",
				() =>
				{
					return linuxCpuUtilization.GetCpuUtilization();
				},
				unit: "%",
				description: "System-wide CPU utilization in percentage.");
		}
	}

	public ExtendedSystemMetrics()
	{
	}
}
