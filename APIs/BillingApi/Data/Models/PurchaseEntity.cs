using BillingApi.Enums;

namespace BillingApi.Data.Models;

public class PurchaseEntity
{
    public int Id { get; set; }    
    public int OrderId { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Creditcard { get; set; } = string.Empty;
	public string Cvc { get; set; } = string.Empty;
	public string Expiration { get; set; } = string.Empty;
	public decimal TotalCost { get; set; }
	public Status Status { get; set; }
}
