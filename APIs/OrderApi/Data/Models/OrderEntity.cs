using OrderApi.Enums;

namespace OrderApi.Data.Models;

public class OrderEntity
{
	public int Id { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Address { get; set; }
	public string Expiration { get; set; }
	public string Creditcard { get; set; }
	public string Cvc { get; set; }
	public List<ProductEntity> ProductEntities { get; set; } = new();
    public Status Status { get; set; }
}
