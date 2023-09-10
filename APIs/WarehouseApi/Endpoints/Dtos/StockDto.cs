using WarehouseApi.Enums;

namespace WarehouseApi.Endpoints.Dtos;

public record StockDto(
	int Id,
	string Sku,
	string Name,
	decimal Cost,
	Status Status);
