namespace StockManagementWebsite.Shared;

public record ProductDto(
	int ProductId, // consider product identifier
	string Sku,
	string Name,
	decimal Cost,
	int Quantity);