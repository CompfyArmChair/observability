using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace BasketApi;

public class TelemetryInitializer : ITelemetryInitializer
{
	public void Initialize(ITelemetry telemetry)
	{
		if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
		{			
			telemetry.Context.Cloud.RoleName = "Basket Role Name";
			telemetry.Context.Cloud.RoleInstance = "Basket Role Instance";
		}
	}
}
