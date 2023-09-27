namespace StockManagementWebsite.Shared.StockCategories;

public record AddStockCategoryDto(
    string Sku,
    string Name,
    decimal Cost);
