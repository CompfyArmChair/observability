using Microsoft.EntityFrameworkCore;
using SalesApi.Data.Models;

namespace SalesApi.Data;

public class SalesDbContext : DbContext
{
    public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options)
    {
    }

    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>()
            .ToTable("products", schema: "sales")
            .Property(e => e.Cost)
            .HasColumnType("money");
    }
}
