using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Endpoints.Dtos;
using WarehouseApi.Enums;

namespace WarehouseApi.Endpoints;

public class OrderStockEndpoint : Endpoint<OrderStockRequest, EmptyResponse>
{
    private readonly WarehouseDbContext _dbContext;

    public OrderStockEndpoint(WarehouseDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/v2/Stock/Command/OrderStock");
        AllowAnonymous();
    }

    public async override Task HandleAsync(OrderStockRequest req, CancellationToken ct)
    {
        var stockIds = req.Stock.Select(x => x.Id);

        var existingStock = await _dbContext
            .Stock
            .Where(x => stockIds.Contains(x.Id))
            .ToArrayAsync() ?? Array.Empty<StockEntity>();

        foreach (var existingStockItem in existingStock)
        {
            existingStockItem.Status = Status.Ordered;
        }

        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class OrderStockRequest
{
    public StockDto[] Stock { get; set; } = Array.Empty<StockDto>();
}


