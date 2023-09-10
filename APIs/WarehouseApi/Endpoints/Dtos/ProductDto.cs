using WarehouseApi.Enums;

namespace WarehouseApi.Endpoints.Dtos;

public record ProductDto(	
	string Sku,
	int Quantity);
