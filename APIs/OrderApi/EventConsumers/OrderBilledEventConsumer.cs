using MassTransit;
using OrderApi.Data;
using OrderApi.Enums;
using Shared.ServiceBus.Commands;
using Shared.ServiceBus.Events;

namespace OrderApi.EventConsumers;

public class OrderBilledEventConsumer : IConsumer<OrderBilledEvent>
{
	private readonly OrderDbContext _dbContext;

	public OrderBilledEventConsumer(OrderDbContext dbContext)
		=> _dbContext = dbContext;

	public async Task Consume(ConsumeContext<OrderBilledEvent> context)
	{
		var orderId = context.Message.OrderId;
		var order = _dbContext.Orders.Single(x => x.Id == orderId);

		order.Status = order.Status switch
		{
			Status.Ordered => Status.Paid,
			Status.Paid => throw new InvalidOperationException($"Order {orderId} has already been paid for"),
			Status.Shipped => throw new InvalidOperationException($"Order {orderId} has already been shipped"),
			_ => throw new NotImplementedException(),
		};

		await context.Send(new Uri("queue:EmailApi"), new SendMailCommand()
		{
			Firstname = order.FirstName,
			Lastname = order.LastName,
			Email = order.Email,
			Subject = "Order billed",
			Body = $"Hello {order.FirstName}, you have been billed for your order"
		});

		await _dbContext.SaveChangesAsync();
	}
}
