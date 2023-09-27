using WarehouseApi.Enums;

namespace WarehouseApi.Data.Models;

public class StockEntity
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public DateTime DateOfAddition { get; set; }
    public Status Status { get; set; }
}
