namespace StockManagementWebsite.Shared.StockCategories;

public record StockCategoryDto(
    string Sku,
    string Name,
    decimal Cost,
    int Quantity);
