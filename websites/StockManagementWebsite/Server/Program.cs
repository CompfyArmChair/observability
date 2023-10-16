using MassTransit;
using Shared.Instrumentation;
using Shared.ServiceBus;
using StockManagementWebsite.Server.Hubs;
using StockManagementWebsite.Shared.StockCategories;
using StockManagementWebsite.Shared.StockItems;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

builder.Services.AddSignalR();

builder.Services.AddMassTransit(config =>
    config.AddDefault("StockManagementWebsite.Server", builder.Configuration.GetConnectionString("ServiceBus")!));

builder.Services
	.AddOpenTelemetry("StockManagementBFF", builder.Configuration.GetConnectionString("ApplicationInsights")!)
		.WithBaggage<AddStockCategoryDto>("stockmanagement.website.category.sku", category => category.Sku.ToString())
		.WithBaggage<EditStockCategoryDto>("stockmanagement.website.category.sku", category => category.Sku.ToString())
		.WithBaggage<StockCategoryDto>("stockmanagement.website.category.sku", category => category.Sku)
		.WithBaggage<StockCategoryDto>("stockmanagement.website.category.quantity", category => category.Quantity.ToString())
		.WithBaggage<AddStockItemsDto>("stockmanagement.website.stock.sku", stock => stock.Sku.ToString())
		.WithBaggage<AddStockItemsDto>("stockmanagement.website.stock.quantity", stock => stock.Quantity.ToString())		
		.WithBaggage<StockItemDto>("stockmanagement.website.stock.sku", stock => stock.Sku)
		.WithBaggage<StockItemDto>("stockmanagement.website.stock.status", stock => stock.Status.ToString());

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapHub<StockManagementHub>("/StockManagementHub");
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
