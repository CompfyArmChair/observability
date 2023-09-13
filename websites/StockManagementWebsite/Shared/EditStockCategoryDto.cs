namespace StockManagementWebsite.Shared;

public record EditStockCategoryDto(
    int Id,
    string Sku,
    string Name,
    decimal Cost);
