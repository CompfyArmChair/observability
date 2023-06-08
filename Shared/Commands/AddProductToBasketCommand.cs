namespace Shared.ServiceBus.Commands; 

public class AddProductToBasketCommand
{
	public int BasketId { get; set; }
	public string Sku { get; set; }
	public string Name { get; set; }
	public decimal Cost { get; set; }
	public int Quantity { get; set; }
}
