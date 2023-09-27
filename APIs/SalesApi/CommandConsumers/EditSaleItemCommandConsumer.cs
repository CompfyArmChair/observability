using MassTransit;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using Shared.ServiceBus.Commands;

namespace CatalogueApi.CommandConsumers;

public class EditSaleItemCommandConsumer : IConsumer<EditSaleItemCommand>
{
    private readonly SalesDbContext _dbContext;

    public EditSaleItemCommandConsumer(SalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<EditSaleItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Products.SingleAsync(c => c.Sku == message.Sku);

        existingItem.Cost = message.Cost;

        await _dbContext.SaveChangesAsync();
    }
}
