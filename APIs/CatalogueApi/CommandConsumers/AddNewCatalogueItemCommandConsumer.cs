using CatalogueApi.Data;
using CatalogueApi.Data.Models;
using CatalogueApi.Instrumentation;
using MassTransit;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace CatalogueApi.CommandConsumers;

public class AddNewCatalogueItemCommandConsumer : IConsumer<AddNewCatalogueItemCommand>
{
    private readonly CatalogueDbContext _dbContext;
	private readonly OtelMeters _meters;
	
    public AddNewCatalogueItemCommandConsumer(CatalogueDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
		_meters = meters;
	}

    public async Task Consume(ConsumeContext<AddNewCatalogueItemCommand> context)
    {
        var message = context.Message;

        var newCatalogueItem = new CatalogueEntity()
        {
            Name = message.Name,
            Sku = message.Sku,
        };

        _dbContext.Add(newCatalogueItem);
        await _dbContext.SaveChangesAsync();
		await context.Publish(new CategoriesChangedEvent());
		_meters.AddCategory();
		_meters.IncreaseTotalCategories();
	}
}
