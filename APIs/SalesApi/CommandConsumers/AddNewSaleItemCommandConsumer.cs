using MassTransit;
using SalesApi.Data;
using SalesApi.Data.Models;
using Shared.ServiceBus.Commands;

namespace CatalogueApi.CommandConsumers;

public class AddNewSaleItemCommandConsumer : IConsumer<AddNewSaleItemCommand>
{
    private readonly SalesDbContext _dbContext;

    public AddNewSaleItemCommandConsumer(SalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<AddNewSaleItemCommand> context)
    {
        var message = context.Message;

        var newProduct = new ProductEntity()
        {
            Sku = message.Sku,
            Cost = message.Cost
        };

        _dbContext.Add(newProduct);
        await _dbContext.SaveChangesAsync();
    }
}
