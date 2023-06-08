namespace ShopWebsite.Shared;

public record ProductDto(    
    string Sku,
    string Name,
    decimal Cost,
    int Quantity);