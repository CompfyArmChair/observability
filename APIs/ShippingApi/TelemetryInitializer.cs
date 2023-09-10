using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace ShippingApi;

public class TelemetryInitializer : ITelemetryInitializer
{
	public void Initialize(ITelemetry telemetry)
	{
		if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
		{						
			telemetry.Context.Cloud.RoleName = "Shipping Role Name";
			telemetry.Context.Cloud.RoleInstance = "Shipping Role Instance";
		}
	}
}
