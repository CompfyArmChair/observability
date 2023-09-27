using WarehouseApi.Enums;

namespace WarehouseApi.Endpoints.Dtos;

public record StockDto(
	int Id,
	string Sku,
    DateTime DateOfAddition,
    Status Status);
