using CatalogueApi.Data;
using CatalogueApi.Data.Models;
using MassTransit;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace CatalogueApi.CommandConsumers;

public class AddNewCatalogueItemCommandConsumer : IConsumer<AddNewCatalogueItemCommand>
{
    private readonly CatalogueDbContext _dbContext;
    public AddNewCatalogueItemCommandConsumer(CatalogueDbContext dbContext)
    {
        _dbContext = dbContext;
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
	}
}
