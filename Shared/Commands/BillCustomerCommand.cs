namespace Shared.ServiceBus.Commands;

public class BillCustomerCommand
{
	public int OrderId { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address { get; set; }
	public string Creditcard { get; set; }
	public string Cvc { get; set; }
	public string Expiration { get; set; }
	public decimal TotalCost { get; set; }
}
