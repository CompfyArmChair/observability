using ShippingApi.Enums;

namespace ShippingApi.Data.Models;

public class ShipmentEntity
{
	public int Id { get; set; }
	public int OrderId { get; set; }
	public string Email { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public Status Status{ get; set; }
}
