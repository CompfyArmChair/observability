namespace Shared.ServiceBus.Commands;

public class MakePurchaseCommand
{
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address { get; set; }
	public string Expiration { get; set; }
	public string Creditcard { get; set; }
	public string Cvc { get; set; }
	public BasketCommandDto Basket { get; set; }
}

public record BasketCommandDto(
	int BasketId,
	ProductCommandDto[] Products);

public record ProductCommandDto(
	string Sku,
	string Name,
	decimal Cost,
	int Quantity);
