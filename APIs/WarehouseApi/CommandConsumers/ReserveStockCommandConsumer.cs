using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using System.Diagnostics.Metrics;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Enums;
using WarehouseApi.Instrumentation;

namespace WarehouseApi.CommandConsumers;

public class ReserveStockCommandConsumer : IConsumer<ReserveStockCommand>
{
	private readonly WarehouseDbContext _dbContext;
	private readonly OtelMeters _meters;

	public ReserveStockCommandConsumer(WarehouseDbContext dbContext, OtelMeters meters)
	{
		_dbContext = dbContext;
		_meters = meters;
	}

	public async Task Consume(ConsumeContext<ReserveStockCommand> context)
	{
		var message = context.Message;

		var results = new List<StockEntity>();

		foreach (var stock in message.Stock)
		{
			var sku = stock.Sku;
			var quantity = stock.Quantity;

			var stockItems = await _dbContext
				.Stock
				.Where(x => 
					x.Sku == sku && 
					x.Status == Status.Available)
				.Take(quantity)
				.ToArrayAsync();
			
			if(stockItems.Length != quantity)
			{
				throw new InvalidOperationException($"One or many items of {sku} are not available");
			}

			results.AddRange(stockItems);
		}

		results.ForEach(x => x.Status = Status.Ordered);

		await _dbContext.SaveChangesAsync();
		var totalQuantity = message.Stock.Sum(x => x.Quantity);
		_meters.AddReserveStock(totalQuantity);
		_meters.IncreaseTotalReservedStock(totalQuantity);

		await Task.WhenAll(
			context.Send(new Uri("queue:BillingApi"), new CompleteCustomerBillingCommand() { OrderId = message.OrderId }));
	}
}
