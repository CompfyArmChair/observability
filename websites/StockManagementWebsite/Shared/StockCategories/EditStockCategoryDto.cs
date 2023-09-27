namespace StockManagementWebsite.Shared.StockCategories;

public record EditStockCategoryDto(
    string Sku,
    string Name,
    decimal Cost);
