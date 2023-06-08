using CatalogueApi.Data.Models;

namespace CatalogueApi.Data;

public static class DbInitializer
{
    public static void Initialize(CatalogueDbContext context)
    {
        if (context.Catalogue.Any())
            return;

        var catalogue = new CatalogueEntity[]
        {
            new CatalogueEntity{Sku="com-key",Name="Keyboard"},
            new CatalogueEntity{Sku="com-mon",Name="Monitor"},
            new CatalogueEntity{Sku="com-mou",Name="Mouse"},
            new CatalogueEntity{Sku="com-des",Name="Desk"},
            new CatalogueEntity{Sku="com-tow",Name="Tower"},
            new CatalogueEntity{Sku="com-cpu",Name="CPU"},
            new CatalogueEntity{Sku="com-ram",Name="RAM"},
            new CatalogueEntity{Sku="com-mot",Name="Motherboard"}
        };

        context.Catalogue.AddRange(catalogue);

        context.SaveChanges();
    }
}
