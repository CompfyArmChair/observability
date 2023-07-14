using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace OrderApi;

public class TelemetryInitializer : ITelemetryInitializer
{
	public void Initialize(ITelemetry telemetry)
	{
		if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
		{			
			telemetry.Context.Cloud.RoleName = "Email Role Name";
			telemetry.Context.Cloud.RoleInstance = "Email Role Instance";
		}
	}
}
