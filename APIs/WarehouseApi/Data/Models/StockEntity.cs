using WarehouseApi.Enums;

namespace WarehouseApi.Data.Models;

public class StockEntity
{
    public int Id { get; set; }
    public string Sku { get; set; }
    public Status Status { get; set; }
}
