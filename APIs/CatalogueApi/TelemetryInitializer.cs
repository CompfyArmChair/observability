using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace CatalogueApi;

public class TelemetryInitializer : ITelemetryInitializer
{
	public void Initialize(ITelemetry telemetry)
	{
		if (string.IsNullOrEmpty(telemetry.Context.Cloud.RoleName))
		{			
			telemetry.Context.Cloud.RoleName = "Catalogue Role Name";
			telemetry.Context.Cloud.RoleInstance = "Catalogue Role Instance";
		}
	}
}
