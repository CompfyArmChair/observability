using Microsoft.EntityFrameworkCore;
using ShippingApi.Data.Models;

namespace ShippingApi.Data;

public class ShippingDbContext : DbContext
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
    {
    }

    public DbSet<ShippingEntity> Shippings { get; set; }        
}
