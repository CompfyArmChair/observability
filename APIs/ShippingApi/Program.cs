global using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using Shared.Instrumentation.Metrics;
using Shared.ServiceBus;
using ShippingApi.Data;
using ShippingApi.Instrumentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShippingDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(config =>
	config.AddDefault(
		"ShippingApi", 
		builder.Configuration.GetConnectionString("ServiceBus")!, 
		(context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"ShippingApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()));

//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ShippingDbContext>();
	context.Database.EnsureCreated();

	try
	{
		RelationalDatabaseCreator databaseCreator =
				(RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
		databaseCreator.CreateTables();
	}
	catch (SqlException)
	{
		//A SqlException will be thrown if tables already exist. So simply ignore it.
	}
	//DbInitializer.Initialize(context);
}


app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
