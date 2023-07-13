using BasketApi.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace BasketApi.CommandConsumers;

public class ClearBasketCommandConsumer : IConsumer<ClearBasketCommand>
{
	private readonly BasketDbContext _dbContext;

	public ClearBasketCommandConsumer(BasketDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<ClearBasketCommand> context)
	{
		var message = context.Message;
		
		var basket = await _dbContext
			.Baskets
			.Include(x => x.Products)
			.SingleAsync(x => x.Id == message.BasketId);

		_dbContext
			.Baskets
			.Remove(basket);

		await _dbContext.SaveChangesAsync();

		await context.Publish(new BasketChangedEvent() { BasketId = message.BasketId });
	}
}
