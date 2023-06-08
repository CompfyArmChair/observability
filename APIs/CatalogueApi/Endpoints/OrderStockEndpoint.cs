using Microsoft.EntityFrameworkCore;
using CatalogueApi.Data;
using CatalogueApi.Data.Models;
using CatalogueApi.Endpoints.Dtos;

namespace CatalogueApi.Endpoints;

public class OrderSkuEndpoint : Endpoint<OrderSkuRequest, EmptyResponse>
{
    private readonly CatalogueDbContext _dbContext;

    public OrderSkuEndpoint(CatalogueDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Sku/Command/OrderSku");
        AllowAnonymous();
    }

    public async override Task HandleAsync(OrderSkuRequest req, CancellationToken ct)
    {
   //     var SkuIds = req.Sku.Select(x => x.Id);

   //     var existingSku = await _dbContext
   //         .Catalogue
			//.Where(x => SkuIds.Contains(x.Id))
   //         .ToArrayAsync() ?? Array.Empty<CatalogueEntity>();


   //     await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class OrderSkuRequest
{
    public ProductDto[] Sku { get; set; } = Array.Empty<ProductDto>();
}


