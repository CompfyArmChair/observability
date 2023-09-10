using ShippingApi.Data.Models;
using ShippingApi.Data;
using Shared.ServiceBus.Commands;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShippingApi.Enums;
using Shared.ServiceBus.Events;

namespace ShippingApi.CommandConsumers;

public class ShipPurchaseCommandConsumer : IConsumer<ShipPurchaseCommand>
{
	private readonly ShippingDbContext _dbContext;

	public ShipPurchaseCommandConsumer(ShippingDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<ShipPurchaseCommand> context)
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
				Email = message.Email,
				FirstName = message.FirstName,
				LastName = message.LastName,
				Address = message.Address,
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
