namespace CatalogueApi.Data.Models;

public class CatalogueEntity
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
}
