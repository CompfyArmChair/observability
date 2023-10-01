namespace Shared.ServiceBus.Commands;

public class BillCustomerCommand
{
	public Guid OrderId { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Creditcard { get; set; } = string.Empty;
	public string Cvc { get; set; } = string.Empty;
	public string Expiration { get; set; } = string.Empty;
	public decimal TotalCost { get; set; }
    public string CustomerReference { get; set; } = string.Empty;
}
