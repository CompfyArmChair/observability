using MassTransit;
using SalesApi.Data;
using SalesApi.Data.Models;
using SalesApi.Instrumentation;
using Shared.ServiceBus.Commands;

namespace CatalogueApi.CommandConsumers;

public class AddNewSaleItemCommandConsumer : IConsumer<AddNewSaleItemCommand>
{
    private readonly SalesDbContext _dbContext;
	private readonly OtelMeters _meters;

	public AddNewSaleItemCommandConsumer(SalesDbContext dbContext, OtelMeters meters)
    {
        _dbContext = dbContext;
		_meters = meters;
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
        _meters.AddPrice();        
    }
}
