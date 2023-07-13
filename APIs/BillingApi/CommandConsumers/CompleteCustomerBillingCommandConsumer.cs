using BillingApi.Data.Models;
using BillingApi.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using BillingApi.Enums;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace BillingApi.CommandConsumers;

public class CompleteCustomerBillingCommandConsumer : IConsumer<CompleteCustomerBillingCommand>
{
    private readonly BillingDbContext _dbContext;

    public CompleteCustomerBillingCommandConsumer(BillingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<CompleteCustomerBillingCommand> context)
    {
        var message = context.Message;

        var purchase = await _dbContext
            .Purchases
            .SingleOrDefaultAsync(x => x.OrderId == message.OrderId);

        if (purchase is not null)
        {
            if (purchase.Status == Status.Processing)
            {
                var externalApiResult = await CallExternalApi(purchase);

                purchase.Status = externalApiResult ? Status.Successful : Status.Failed;
                await _dbContext.SaveChangesAsync();

				await context.Publish(new OrderBilledEvent()
				{
					OrderId = message.OrderId,
				});
			}
        }
        else
        {
            _dbContext.Purchases.Add(new PurchaseEntity
            {
                OrderId = message.OrderId,
                Status = Status.Processing,
            });

            await _dbContext.SaveChangesAsync();
        }
    }

    private Task<bool> CallExternalApi(PurchaseEntity purchase)
    {
        return Task.FromResult(true);
    }
}
