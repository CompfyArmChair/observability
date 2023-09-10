namespace BasketApi.Data.Models;

public class BasketEntity
{
    public int Id { get; set; }    
    public List<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
