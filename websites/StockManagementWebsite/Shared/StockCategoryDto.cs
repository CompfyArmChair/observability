namespace StockManagementWebsite.Shared;

public record StockCategoryDto(
    int Id,
    string Sku,
    string Name,
    decimal Cost,
    int Quantity);
