using MassTransit;
using OrderApi.Data;
using OrderApi.Data.Models;
using OrderApi.Enums;
using OrderApi.Instrumentation;
using Shared.ServiceBus.Commands;

namespace BasketApi.CommandConsumers;

public class MakePurchaseCommandConsumer : IConsumer<MakePurchaseCommand>
{
	private readonly OrderDbContext _dbContext;
	private readonly OtelMeters _meters;

	public MakePurchaseCommandConsumer(OrderDbContext dbContext, OtelMeters meters)
	{
		_dbContext = dbContext;
		_meters = meters;
	}

	public async Task Consume(ConsumeContext<MakePurchaseCommand> context)
	{
		var message = context.Message;

		var newPurchase = new OrderEntity()
		{
			CustomerReference = $"{DateTime.UtcNow:yyMMddTHHmmss}-{message.Basket.BasketId}",
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
		_meters.AddOrder();
		_meters.IncreaseOrders();
		_meters.RecordNumberOfProducts(newPurchase.ProductEntities.Count());

		await Task.WhenAll(
			context.Send(new Uri("queue:EmailApi"), new SendMailCommand() 
			{ 
				Firstname = message.FirstName,
				Lastname = message.LastName,
				Email = message.Email,
				Reference = newPurchase.CustomerReference,
				Subject = "Order Received",
				Body = $"Hello {message.FirstName}, your order has been received. Reference: {newPurchase.CustomerReference}"
			}),
			context.Send(new Uri("queue:BasketApi"), new ClearBasketCommand() { BasketId = message.Basket.BasketId }),
			context.Send(new Uri("queue:WarehouseApi"), new ReserveStockCommand() 
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
			context.Send(new Uri("queue:BillingApi"), new BillCustomerCommand()
			{
				OrderId = newPurchase.Id,
				CustomerReference = newPurchase.CustomerReference,
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
