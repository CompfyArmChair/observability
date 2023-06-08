using BasketApi.Data;
using BasketApi.Data.Models;
using BillingApi.Data;
using BillingApi.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace BasketApi.CommandConsumers;

public class BillCustomerCommandConsumer : IConsumer<BillCustomerCommand>
{
	private readonly BillingDbContext _dbContext;

	public BillCustomerCommandConsumer(BillingDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Consume(ConsumeContext<BillCustomerCommand> context)
	{
		var message = context.Message;

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
	}
}
