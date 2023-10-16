using WarehouseApi.Data.Models;
using WarehouseApi.Enums;

namespace WarehouseApi.Data;

public static class DbInitializer
{
    public static void Initialize(WarehouseDbContext context)
    {
        if (context.Stock.Any())
            return;

        var stock = new StockEntity[]
        {
                new StockEntity{Sku="com-key",Status = Status.Available},
                new StockEntity{Sku="com-mon",Status = Status.Available},
                new StockEntity{Sku="com-mou",Status = Status.Available},
                new StockEntity{Sku="com-des",Status = Status.Available},
                new StockEntity{Sku="com-tow",Status = Status.Available},
                new StockEntity{Sku="com-cpu",Status = Status.Available},
                new StockEntity{Sku="com-ram",Status = Status.Available},
                new StockEntity{Sku="com-mot",Status = Status.Available},
				new StockEntity{Sku="com-net",Status = Status.Available},
				new StockEntity{Sku="com-pdp",Status = Status.Available}
		};

        context.Stock.AddRange(stock);
        context.SaveChanges();
    }
}
