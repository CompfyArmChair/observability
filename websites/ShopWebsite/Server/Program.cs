using MassTransit;
using Shared.Instrumentation;
using Shared.ServiceBus;
using ShopWebsite.Server.Hubs;

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
                config.AddDefault("ShopWebsite.Server", builder.Configuration.GetConnectionString("ServiceBus")!));

			builder.Services.AddOpenTelemetry("ShopBFF", builder.Configuration.GetConnectionString("ApplicationInsights")!);

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