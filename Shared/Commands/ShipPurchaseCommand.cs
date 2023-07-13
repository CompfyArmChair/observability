namespace Shared.ServiceBus.Commands;

public class ShipPurchaseCommand
{
	public int OrderId { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;	
}
