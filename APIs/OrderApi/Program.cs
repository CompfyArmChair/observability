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
using Microsoft.ApplicationInsights.Extensibility;
using OrderApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddInstrumentation();
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddMassTransit(config =>
	config.AddDefault("OrderApi"));

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
