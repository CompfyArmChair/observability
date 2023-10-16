global using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using OrderApi.Data;
using OrderApi.Instrumentation;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using Shared.Instrumentation.Metrics;
using Shared.ServiceBus;
using Shared.ServiceBus.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

//builder.Services.AddInsightsTelemetry();
//builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddMassTransit(config =>
	config.AddDefault(
		"OrderApi", 
		builder.Configuration.GetConnectionString("ServiceBus")!, 
		(context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

builder.Services.AddOpenTelemetry(
	"OrderApi", 
	builder.Configuration.GetConnectionString("ApplicationInsights")!,
	new OtelMetricsConfiguration<OtelMeters>(new OtelMeters()))
    .WithBaggage<SendMailCommand>("shop.orderapi.mail.ref", command => command.Reference)
    .WithBaggage<ReserveStockCommand>("shop.orderapi.order.id", command => command.OrderId.ToString())
    .WithBaggage<BillCustomerCommand>("shop.orderapi.order.id", command => command.OrderId.ToString())
    .WithBaggage<BillCustomerCommand>("shop.orderapi.customer.ref", command => command.CustomerReference);

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<OrderDbContext>();
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
