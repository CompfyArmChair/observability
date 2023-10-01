using Azure.Identity;
using MassTransit;
using System.Reflection;

namespace Shared.ServiceBus
{
	public static class ServiceBusExtensionMethods
	{
		public static void AddDefault(this IBusRegistrationConfigurator config, string serviceName, string connectionString, Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator>? configure = null)
		{
			Assembly[] assemblies = GetConsumerAssemblies();
			config.AddConsumers(assemblies);
			config.AddServiceBusMessageScheduler();

			config.UsingAzureServiceBus(delegate (IBusRegistrationContext context, IServiceBusBusFactoryConfigurator asbCfg)
			{
				asbCfg.Host(connectionString);
				asbCfg.UseServiceBusMessageScheduler();
				asbCfg.UseMessageRetry(delegate (IRetryConfigurator rtCfg)
				{
					rtCfg.Exponential(3, TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(1.0));
				});

				asbCfg.ReceiveEndpoint(serviceName, delegate (IServiceBusReceiveEndpointConfigurator reCfg)
				{
					reCfg.ConfigureDeadLetterQueueErrorTransport();
					reCfg.EnableDuplicateDetection(TimeSpan.FromMinutes(10.0));
					reCfg.PublishFaults = false;
					reCfg.DiscardSkippedMessages();
					reCfg.ConfigureConsumers(context);
				});

				asbCfg.ConfigureEndpoints(context);
				asbCfg.MaxAutoRenewDuration = TimeSpan.FromMinutes(30.0);

				configure?.Invoke(context, asbCfg);
			});

			static Assembly[] GetConsumerAssemblies()
			{
				var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();
				var list = new List<Assembly>() { assembly };
				var array = assembly.GetName().Name!.Split(',');

				if (array.Length != 0)
				{
					var text = array[0];
					var collection = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, text + "*dll")
						.Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));

					list.AddRange(collection);
				}

				return list.Distinct().ToArray();
			}
		}

        public static IServiceBusBusFactoryConfigurator UseSendAndPublishFilter(
            this IServiceBusBusFactoryConfigurator serviceBusBusFactoryConfigurator, Type type,
            IBusRegistrationContext context)
        {
            serviceBusBusFactoryConfigurator.UseSendFilter(type, context);
            serviceBusBusFactoryConfigurator.UsePublishFilter(type, context);
            return serviceBusBusFactoryConfigurator;
        }
    }
}