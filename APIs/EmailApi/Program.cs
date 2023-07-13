using MassTransit;
using Shared.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
	config.AddDefault("EmailApi"));

var app = builder.Build();

app.Run();
