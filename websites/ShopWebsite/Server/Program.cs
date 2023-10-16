using MassTransit;
using Shared.Instrumentation;
using Shared.Instrumentation.MassTransit;
using Shared.ServiceBus;
using ShopWebsite.Server.Hubs;
using ShopWebsite.Shared;

namespace ShopWebsite
{
	public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
			builder.Services.AddHttpClient();

			builder.Services.AddSignalR();

			builder.Services.AddMassTransit(config => 
                config.AddDefault("ShopWebsite.Server", builder.Configuration.GetConnectionString("ServiceBus")!, (context, configurator) => configurator.UseSendAndPublishFilter(typeof(TelemetrySendFilter<>), context)));

            builder.Services
                .AddOpenTelemetry("ShopBFF", builder.Configuration.GetConnectionString("ApplicationInsights")!)
                .WithBaggage<BasketDto>("shop.website.basket.id", basket => basket.Id.ToString())
                .WithBaggage<BasketDto>("shop.website.basket.contents", basket => string.Join(',', basket.Products.Select(x => x.Name)))
                .WithBaggage<ProductDto>("shop.website.product.name", product => product.Name)
                .WithBaggage<ProductDto>("shop.website.product.quantity", product => product.Quantity.ToString())
                .WithBaggage<ProductDto>("shop.website.product.sku", product => product.Sku);

			builder.Services.AddHealthChecks();

			var app = builder.Build();

			app.MapHealthChecks("/health");

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
			app.MapHub<ShopHub>("/shophub");
			app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}