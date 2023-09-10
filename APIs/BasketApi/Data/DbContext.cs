using BasketApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BasketApi.Data;

public class BasketDbContext : DbContext
{
    public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
    {
    }

    public DbSet<BasketEntity> Baskets { get; set; }
    public DbSet<ProductEntity> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BasketEntity>()
            .ToTable("baskets", schema: "basket");

        modelBuilder.Entity<ProductEntity>()
            .ToTable("products", schema: "basket");

		modelBuilder.Entity<ProductEntity>()
			.Property(e => e.Cost)
			.HasColumnType("money");

		modelBuilder.Entity<BasketEntity>()
            .HasMany(b => b.Products)
            .WithOne();

        modelBuilder.Entity<BasketEntity>()
            .Navigation(b => b.Products)
            .UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}
