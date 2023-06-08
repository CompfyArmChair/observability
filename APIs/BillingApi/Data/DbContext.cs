using Microsoft.EntityFrameworkCore;
using BillingApi.Data.Models;

namespace BillingApi.Data;

public class BillingDbContext : DbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
    {
    }

    public DbSet<PurchaseEntity> Purchases { get; set; }        
}
