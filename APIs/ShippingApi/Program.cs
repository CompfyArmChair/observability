global using FastEndpoints;
using FastEndpoints.Swagger;
using ShippingApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Data.SqlClient;
using MassTransit;
using Shared.ServiceBus;
using Shared.Instrumentation;
using Microsoft.ApplicationInsights.Extensibility;
using ShippingApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShippingDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(config =>
	config.AddDefault("ShippingApi"));

builder.Services.AddInstrumentation();
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

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
