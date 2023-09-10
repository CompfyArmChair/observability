namespace Shared.ServiceBus.Commands;

public class MakePurchaseCommand
{
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Expiration { get; set; } = string.Empty;
	public string Creditcard { get; set; } = string.Empty;
	public string Cvc { get; set; } = string.Empty;
	public BasketCommandDto Basket { get; set; } = default!;
}

public class BasketCommandDto
{
	public int BasketId { get;set; }
	public ProductCommandDto[] Products { get; set; } = Array.Empty<ProductCommandDto>();

}

public class ProductCommandDto
{
	public string Sku { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public decimal Cost { get; set; }
	public int Quantity { get; set; }
}
