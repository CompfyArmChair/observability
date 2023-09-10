using Microsoft.EntityFrameworkCore;
using CatalogueApi.Data.Models;

namespace CatalogueApi.Data;

public class CatalogueDbContext : DbContext
{
    public CatalogueDbContext(DbContextOptions<CatalogueDbContext> options) : base(options)
    {
    }
    
    public DbSet<CatalogueEntity> Catalogue { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<CatalogueEntity>()
			.ToTable("catalogue", schema: "catalogue");
	}
}
