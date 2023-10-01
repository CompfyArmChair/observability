using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;
using ShippingApi.Data;
using ShippingApi.Data.Models;
using ShippingApi.Enums;
using ShippingApi.Instrumentation;

namespace BillingApi.CommandConsumers;

public class CompleteShipPurchaseCommandConsumer : IConsumer<CompleteShipPurchaseCommand>
{
	private readonly ShippingDbContext _dbContext;
	private readonly OtelMeters _meters;

	public CompleteShipPurchaseCommandConsumer(ShippingDbContext dbContext, OtelMeters meters)
	{
		_dbContext = dbContext;
		_meters = meters;
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
					_meters.CompleteShipment();
					_meters.IncreaseTotalShipmentsCompleted();
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

			_meters.AddShipment();
			_meters.IncreaseTotalShipments();
		}
	}

	private Task<bool> CallExternalApi(ShipmentEntity shipment)
	{
		return Task.FromResult(true);
	}
}
