using MassTransit;
using OrderApi.Data;
using OrderApi.Data.Models;
using OrderApi.Enums;
using Shared.ServiceBus.Commands;

namespace BasketApi.CommandConsumers;

public class MakePurchaseCommandConsumer : IConsumer<MakePurchaseCommand>
{
	private readonly OrderDbContext _dbContext;

	public MakePurchaseCommandConsumer(OrderDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<MakePurchaseCommand> context)
	{
		var message = context.Message;

		var newPurchase = new OrderEntity()
		{
			Email = message.Email,
			FirstName = message.FirstName,
			LastName = message.LastName,
			Address = message.Address,
			Creditcard = message.Creditcard,
			Cvc = message.Cvc,
			Expiration = message.Expiration,
			Status = Status.Ordered,
			ProductEntities = message.Basket.Products
				.Select(x => new ProductEntity()
				{
					Sku = x.Sku,
					Name = x.Name,
					Cost = x.Cost,
					Quantity = x.Quantity
				})
				.ToList()
		};

		_dbContext.Orders.Add(newPurchase);

		await _dbContext.SaveChangesAsync();

		await Task.WhenAll(
			context.Send(new SendMailCommand() 
			{ 
				Firstname = message.FirstName,
				Lastname = message.LastName,
				Email = message.Email,
				Subject = "Order Recieved",
				Body = $"Hello {message.FirstName}, your order has been recieved"
			}),
			context.Send(new ClearBasketCommand() { BasketId = message.Basket.BasketId }),
			context.Send(new ReserveStockCommand() 
			{
				OrderId = newPurchase.Id,
				Stock = message.Basket.Products
					.Select(x => 
						new ReservedStock() 
						{ 
							Sku = x.Sku, 
							Quantity = x.Quantity 
						})
					.ToArray()
			}),
			context.Send(new BillCustomerCommand()
			{
				OrderId = newPurchase.Id,
				Email = message.Email,
				FirstName = message.FirstName,
				LastName = message.LastName,
				Address = message.Address,
				Creditcard = message.Creditcard,
				Cvc = message.Cvc,
				Expiration = message.Expiration,
				TotalCost = message.Basket.Products.Sum(x => x.Cost)
			}));
	}
}
