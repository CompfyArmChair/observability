namespace BasketApi.Endpoints.Dtos;

public record ProductDto(
	string Sku,
	string Name,
	decimal Cost,
	int Quantity);
