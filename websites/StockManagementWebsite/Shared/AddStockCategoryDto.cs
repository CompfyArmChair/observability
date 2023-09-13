namespace StockManagementWebsite.Shared;

public record AddStockCategoryDto(    
    string Sku,
    string Name,
    decimal Cost);
