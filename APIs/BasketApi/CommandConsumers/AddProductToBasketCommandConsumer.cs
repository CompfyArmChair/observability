using BasketApi.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace BasketApi.CommandConsumers;

public class AddProductToBasketCommandConsumer : IConsumer<AddProductToBasketCommand>
{
	private readonly BasketDbContext _dbContext;

	public AddProductToBasketCommandConsumer(BasketDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<AddProductToBasketCommand> context)
	{
		var message = context.Message;
		
		var basket = await _dbContext
			.Baskets
			.Include(x => x.Products)
			.SingleAsync(x => x.Id == message.BasketId);

		var existingProduct = basket
			.Products
			.SingleOrDefault(x => x.Sku == message.Sku);

		if (existingProduct is null)
		{
			basket.Products.Add(new()
			{
				Quantity = message.Quantity,
				Cost = message.Cost,
				Name = message.Name,
				Sku = message.Sku
			});
		}
		else
		{
			existingProduct.Quantity += message.Quantity;
		}


		await _dbContext.SaveChangesAsync();

		await context.Publish(new BasketChangedEvent() { BasketId = basket.Id });
	}
}
