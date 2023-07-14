using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace Shared.Instrumentation;

public static class InstrumentationExtensionMethods
{
	public static void AddInstrumentation(this IServiceCollection services)
	{
		services.AddApplicationInsightsTelemetry(options =>
		{
			options.ConnectionString = "InstrumentationKey=9407872d-cfec-4884-af99-e3ea35147cda;IngestionEndpoint=https://uksouth-1.in.applicationinsights.azure.com/;LiveEndpoint=https://uksouth.livediagnostics.monitor.azure.com/";
		});		
		//services.AddCloudRoleNameInitializer(Constants.ServiceName);
	}
}