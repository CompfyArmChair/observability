global using FastEndpoints;
using CatalogueApi.Data;
using CatalogueApi.Instrumentation;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Shared.Instrumentation;
using Shared.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogueDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(config =>
    config.AddDefault(
		"CatalogueApi", 
		builder.Configuration.GetConnectionString("ServiceBus")!));

builder.Services.AddOpenTelemetry(
	"CatalogueApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()));
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<CatalogueDbContext>();
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
	DbInitializer.Initialize(context);
}

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
