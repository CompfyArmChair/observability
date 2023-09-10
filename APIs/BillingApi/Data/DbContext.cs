using Microsoft.EntityFrameworkCore;
using BillingApi.Data.Models;

namespace BillingApi.Data;

public class BillingDbContext : DbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
    {
    }

    public DbSet<PurchaseEntity> Purchases { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PurchaseEntity>()
			.ToTable("purchases", schema: "billing");

		modelBuilder.Entity<PurchaseEntity>()
			.Property(e => e.TotalCost)
			.HasColumnType("money");
	}
}
