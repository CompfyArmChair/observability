global using FastEndpoints;
using FastEndpoints.Swagger;
using OrderApi.Data;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus;
using MassTransit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using Shared.ServiceBus.Commands;
using OrderApi.Instrumentation;

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
    .WithBaggage<SendMailCommand>("orderapi.mail.ref", command => command.Reference)
    .WithBaggage<ReserveStockCommand>("orderapi.order.id", command => command.OrderId.ToString())
    .WithBaggage<BillCustomerCommand>("orderapi.order.id", command => command.OrderId.ToString())
    .WithBaggage<BillCustomerCommand>("orderapi.customer.ref", command => command.CustomerReference);

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();

var app = builder.Build();

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
