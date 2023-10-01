global using FastEndpoints;
using BillingApi.Data;
using BillingApi.Instrumentation;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using Shared.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BillingDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(config =>
	config.AddDefault(
		"BillingApi", 
		builder.Configuration.GetConnectionString("ServiceBus")!, 
		(context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"BillingApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()));
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<BillingDbContext>();
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
}

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
