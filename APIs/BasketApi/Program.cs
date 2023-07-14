global using FastEndpoints;
using BasketApi.Data;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.ServiceBus;
using Shared.Instrumentation;
using BasketApi;
using Microsoft.ApplicationInsights.Extensibility;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<BasketDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(config =>
	config.AddDefault("BasketApi"));

builder.Services.AddInstrumentation();
builder.Services.AddSingleton<ITelemetryInitializer, TelemetryInitializer>();

builder.Services.AddSwaggerDoc();
builder.Services.AddFastEndpoints();


var app = builder.Build();

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
