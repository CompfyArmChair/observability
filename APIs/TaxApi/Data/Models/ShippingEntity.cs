using ShippingApi.Enums;

namespace ShippingApi.Data.Models;

public class ShippingEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Status Status{ get; set; }
}
