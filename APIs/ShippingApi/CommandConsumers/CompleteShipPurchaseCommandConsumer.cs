using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;
using ShippingApi.Data;
using ShippingApi.Data.Models;
using ShippingApi.Enums;

namespace BillingApi.CommandConsumers;

public class CompleteShipPurchaseCommandConsumer : IConsumer<CompleteShipPurchaseCommand>
{
	private readonly ShippingDbContext _dbContext;

	public CompleteShipPurchaseCommandConsumer(ShippingDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<CompleteShipPurchaseCommand> context)
	{
		var message = context.Message;

		var shipment = await _dbContext
			.Shipments
			.SingleOrDefaultAsync(x => x.OrderId == message.OrderId);

		if (shipment is not null)
		{
			if (shipment.Status == Status.Pending)
			{
				var externalApiResult = await CallExternalApi(shipment);

				if (externalApiResult)
				{
					shipment.Status = Status.Shipped;
					await _dbContext.SaveChangesAsync();

					await context.Publish(new OrderBilledEvent()
					{
						OrderId = message.OrderId,
					});
				}
			}
		}
		else
		{
			_dbContext.Shipments.Add(new ShipmentEntity
			{
				OrderId = message.OrderId,
				Status = Status.Pending,
			});

			await _dbContext.SaveChangesAsync();
		}
	}

	private Task<bool> CallExternalApi(ShipmentEntity shipment)
	{
		return Task.FromResult(true);
	}
}
