global using FastEndpoints;
using BasketApi.Data;
using BasketApi.Endpoints.Dtos;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.ServiceBus;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using BasketApi.Instrumentation;
using Shared.Instrumentation.Metrics;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<BasketDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(config =>
	config.AddDefault(
		"BasketApi", 
		builder.Configuration.GetConnectionString("ServiceBus")!, 
		(context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"BasketApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()))
		.WithBaggage<BasketDto>("shop.basketapi.basket.id", basket => basket.Id.ToString())
		.WithBaggage<BasketDto>("shop.basketapi.basket.contents",
			basket => string.Join(',', basket.Products.Select(x => x.Sku)));
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	var context = services.GetRequiredService<BasketDbContext>();
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

app.Services.GetRequiredService<IBusControl>().Start();

app.UseFastEndpoints();
app.UseSwaggerGen();

app.Run();
