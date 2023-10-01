using CatalogueApi.Data;
using CatalogueApi.Instrumentation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace CatalogueApi.CommandConsumers;

public class EditCatalogueItemCommandConsumer : IConsumer<EditCatalogueItemCommand>
{
    private readonly CatalogueDbContext _dbContext;
	private readonly OtelMeters _meters;
	public EditCatalogueItemCommandConsumer(CatalogueDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
		_meters = meters;
	}

    public async Task Consume(ConsumeContext<EditCatalogueItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Catalogue.SingleAsync(c => c.Sku == message.Sku);

        existingItem.Name = message.Name;        

        await _dbContext.SaveChangesAsync();
		await context.Publish(new CategoriesChangedEvent());
		_meters.UpdateCategory();
	}
}
