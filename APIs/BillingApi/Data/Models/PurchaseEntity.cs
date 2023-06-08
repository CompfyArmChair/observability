using BillingApi.Enums;

namespace BillingApi.Data.Models;

public class PurchaseEntity
{
    public int Id { get; set; }    
    public int OrderId { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address { get; set; }
	public string Creditcard { get; set; }
	public string Cvc { get; set; }
	public string Expiration { get; set; }
	public decimal TotalCost { get; set; }
	public Status Status { get; set; }
}
