namespace Shared.ServiceBus.Commands; 

public class AddProductToBasketCommand
{
	public int BasketId { get; set; }
	public string Sku { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public decimal Cost { get; set; }
	public int Quantity { get; set; }
}
