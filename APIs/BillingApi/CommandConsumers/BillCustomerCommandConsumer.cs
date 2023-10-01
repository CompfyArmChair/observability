using BillingApi.Data;
using BillingApi.Data.Models;
using BillingApi.Enums;
using BillingApi.Instrumentation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace BillingApi.CommandConsumers;

public class BillCustomerCommandConsumer : IConsumer<BillCustomerCommand>
{
	private readonly BillingDbContext _dbContext;
	private readonly OtelMeters _meters;

	public BillCustomerCommandConsumer(BillingDbContext dbContext, OtelMeters meters)
	{
		_dbContext = dbContext;
		_meters = meters;
	}

	public async Task Consume(ConsumeContext<BillCustomerCommand> context)
	{
		var message = context.Message;

		var purchase = await _dbContext
			.Purchases
			.SingleOrDefaultAsync(x => x.OrderId == message.OrderId);

		if (purchase is not null)
		{
			if (purchase.Status == Status.Processing)
			{
				purchase.FirstName = message.FirstName;
				purchase.LastName = message.LastName;
				purchase.Address = message.Address;
				purchase.Creditcard = message.Creditcard;
				purchase.Cvc = message.Cvc;
				purchase.Email = message.Email;
				purchase.Expiration = message.Expiration;
				purchase.TotalCost = message.TotalCost;

				var externalApiResult = await CallExternalApi(purchase);

				purchase.Status = externalApiResult ? Status.Successful : Status.Failed;
				await _dbContext.SaveChangesAsync();

				if(purchase.Status == Status.Successful)
				{
					_meters.CompleteBilling();
					_meters.IncreaseTotalBillingsCompleted();
				}

				await context.Publish(new OrderBilledEvent()
				{
					OrderId = message.OrderId,
				});
			}			
		}
		else
		{
			_dbContext.Purchases.Add(
				new()
				{
					OrderId = message.OrderId,
					FirstName = message.FirstName,
					LastName = message.LastName,
					Address = message.Address,
					Creditcard = message.Creditcard,
					Cvc = message.Cvc,
					Email = message.Email,
					Expiration = message.Expiration,
					TotalCost = message.TotalCost,
					Status = Status.Processing
				});

			await _dbContext.SaveChangesAsync();
			_meters.AddBilling();
			_meters.IncreaseTotalBillings();

		}
	}

	private Task<bool> CallExternalApi(PurchaseEntity purchase)
	{
		return Task.FromResult(true);
	}
}
