namespace StockManagementWebsite.Shared;

public record StockItemDto(
    int Id,
    string Sku,
    DateTime EntryDate,
    string SerialNumber,
    string AdditionalDetails);