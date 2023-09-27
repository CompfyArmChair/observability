using CatalogueApi.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace CatalogueApi.CommandConsumers;

public class EditCatalogueItemCommandConsumer : IConsumer<EditCatalogueItemCommand>
{
    private readonly CatalogueDbContext _dbContext;
    public EditCatalogueItemCommandConsumer(CatalogueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EditCatalogueItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Catalogue.SingleAsync(c => c.Sku == message.Sku);

        existingItem.Name = message.Name;        

        await _dbContext.SaveChangesAsync();
		await context.Publish(new CategoriesChangedEvent());
	}
}
