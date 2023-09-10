using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Models;

namespace OrderApi.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }

    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<ProductEntity> ProductEntities { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<OrderEntity>()
			.ToTable("orders", schema: "order");

		modelBuilder.Entity<ProductEntity>()
			.ToTable("products", schema: "order");

		modelBuilder.Entity<ProductEntity>()
			.Property(e => e.Cost)
			.HasColumnType("money");

		//modelBuilder.Entity<BasketEntity>()
		//	.HasMany(b => b.Products)
		//	.WithOne();

		//modelBuilder.Entity<BasketEntity>()
		//	.Navigation(b => b.Products)
		//	.UsePropertyAccessMode(PropertyAccessMode.Property);
	}
}
