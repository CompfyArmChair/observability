using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Endpoints.Dtos;
using WarehouseApi.Enums;

namespace WarehouseApi.Endpoints;

public class RemoveStockEndpoint : Endpoint<RemoveStockRequest, EmptyResponse>
{
    private readonly WarehouseDbContext _dbContext;

    public RemoveStockEndpoint(WarehouseDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Stock/Command/RemoveStock");
        AllowAnonymous();
    }

    public async override Task HandleAsync(RemoveStockRequest req, CancellationToken ct)
    {
        var stockIds = req.Stock.Select(x => x.Id);

        var existingStock = await _dbContext
            .Stock
            .Where(x => stockIds.Contains(x.Id))
            .ToArrayAsync() ?? Array.Empty<StockEntity>();

        foreach (var existingStockItem in existingStock)
        {
            existingStockItem.Status = Status.Sold;
        }

        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class RemoveStockRequest
{
    public StockDto[] Stock { get; set; } = Array.Empty<StockDto>();
}


