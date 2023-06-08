namespace BasketApi.Data.Models;

public class ProductEntity
{
    public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public decimal Cost { get; set; }
	public string Sku { get; set; } = string.Empty;
	public int Quantity { get; set; }
}
