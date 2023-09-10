using Microsoft.EntityFrameworkCore;
using ShippingApi.Data.Models;

namespace ShippingApi.Data;

public class ShippingDbContext : DbContext
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
    {
    }

    public DbSet<ShipmentEntity> Shipments { get; set; }        
}
