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
                .WithBaggage<BasketDto>("shopwebsite.basket.id", basket => basket.Id.ToString())
                .WithBaggage<BasketDto>("shopwebsite.basket.contents", basket => string.Join(',', basket.Products.Select(x => x.Name)))
                .WithBaggage<ProductDto>("shopwebsite.product.name", product => product.Name)
                .WithBaggage<ProductDto>("shopwebsite.product.quantity", product => product.Quantity.ToString())
                .WithBaggage<ProductDto>("shopwebsite.product.sku", product => product.Sku);

            var app = builder.Build();            

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