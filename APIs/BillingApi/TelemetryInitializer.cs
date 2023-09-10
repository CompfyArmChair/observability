using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace BillingApi;

public class TelemetryInitializer : ITelemetryInitializer
{
	public void Initialize(ITelemetry telemetry)
	{
		if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
		{			
			telemetry.Context.Cloud.RoleName = "Billing Role Name";
			telemetry.Context.Cloud.RoleInstance = "Billing Role Instance";
		}
	}
}
