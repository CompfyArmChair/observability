namespace StockManagementWebsite.Shared.StockItems;

public record StockItemDto(
    int Id,
    string Sku,
    DateTime DateOfAddition,
    Status Status);