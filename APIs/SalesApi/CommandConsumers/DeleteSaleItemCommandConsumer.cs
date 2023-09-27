using MassTransit;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using Shared.ServiceBus.Commands;

namespace CatalogueApi.CommandConsumers;

public class DeleteSaleItemCommandConsumer : IConsumer<DeleteSaleItemCommand>
{
    private readonly SalesDbContext _dbContext;

    public DeleteSaleItemCommandConsumer(SalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<DeleteSaleItemCommand> context)
    {
        var message = context.Message;

        var existingItem = await _dbContext.Products.SingleAsync(c => c.Sku == message.Sku);

        _dbContext.Remove(existingItem);
        await _dbContext.SaveChangesAsync();
    }
}