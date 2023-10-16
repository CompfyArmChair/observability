using SalesApi.Data.Models;

namespace SalesApi.Data;

public static class DbInitializer
{
    public static void Initialize(SalesDbContext context)
    {
        if (context.Products.Any())
            return;

        var products = new ProductEntity[]
        {
                new ProductEntity{Sku="com-key",Cost=50},
                new ProductEntity{Sku="com-mon",Cost=300},
                new ProductEntity{Sku="com-mou",Cost=30},
                new ProductEntity{Sku="com-des",Cost=130},
                new ProductEntity{Sku="com-tow",Cost=60},
                new ProductEntity{Sku="com-cpu",Cost=230},
                new ProductEntity{Sku="com-ram",Cost=60},
                new ProductEntity{Sku="com-mot",Cost=70},
				new ProductEntity{Sku="com-net",Cost=15},
				new ProductEntity{Sku="com-pdp",Cost=65535}
		};

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}
