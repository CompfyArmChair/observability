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
using Shared.ServiceBus.Commands;
using WarehouseApi.Data;
using WarehouseApi.Instrumentation;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WarehouseDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(config =>
	config.AddDefault(
		"WarehouseApi", 
		builder.Configuration.GetConnectionString("ServiceBus")!, 
		(context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"WarehouseApi",
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()))
		.WithBaggage<AddNewStockItemsCommand>("shop.WarehouseApi.stock.sku", command => command.Sku.ToString())		
		.WithBaggage<DeleteStockItemCommand>("shop.WarehouseApi.stock.sku", command => command.Id.ToString())
		.WithBaggage<ReserveStockCommand>("shop.WarehouseApi.stock.items",
			command => string.Join(',', command.Stock.Select(x => x.Sku)));		
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<WarehouseDbContext>();
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
