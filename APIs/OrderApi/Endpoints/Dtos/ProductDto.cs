namespace OrderApi.Endpoints.Dtos;

public record ProductDto(
    int Id,
	string Sku,
	string Name,
	decimal Cost,
	int Quantity);
