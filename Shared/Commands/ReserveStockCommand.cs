namespace Shared.ServiceBus.Commands;

public class ReserveStockCommand
{
	public int OrderId { get; set; }
	public ReservedStock[] Stock { get; set; } = Array.Empty<ReservedStock>();
}

public class ReservedStock
{
	public string Sku { get; set; } = string.Empty;
	public int Quantity { get; set; }
}