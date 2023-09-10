using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace Salespi;

public class TelemetryInitializer : ITelemetryInitializer
{
	public void Initialize(ITelemetry telemetry)
	{
		if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
		{			
			telemetry.Context.Cloud.RoleName = "Sales Role Name";
			telemetry.Context.Cloud.RoleInstance = "Sales Role Instance";
		}
	}
}
