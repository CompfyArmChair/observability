using CatalogueApi.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace CatalogueApi.CommandConsumers;

public class DeleteCatalogueItemCommandConsumer : IConsumer<DeleteCatalogueItemCommand>
{
    private readonly CatalogueDbContext _dbContext;

    public DeleteCatalogueItemCommandConsumer(CatalogueDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<DeleteCatalogueItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Catalogue.SingleAsync(c => c.Sku == message.Sku);

        _dbContext.Remove(existingItem);
        await _dbContext.SaveChangesAsync();
		await context.Publish(new CategoriesChangedEvent());
	}
}