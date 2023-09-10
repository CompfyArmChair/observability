using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data.Models;

namespace WarehouseApi.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
    {
    }

    public DbSet<StockEntity> Stock { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<StockEntity>()
			.ToTable("stock", schema: "warehouse");
	}
}
