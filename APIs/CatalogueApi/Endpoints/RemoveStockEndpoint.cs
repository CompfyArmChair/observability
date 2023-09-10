using CatalogueApi.Data;
using CatalogueApi.Data.Models;
using CatalogueApi.Endpoints.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CatalogueApi.Endpoints;

public class RemoveSkuEndpoint : Endpoint<RemoveSkuRequest, EmptyResponse>
{
    private readonly CatalogueDbContext _dbContext;

    public RemoveSkuEndpoint(CatalogueDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Sku/Command/RemoveSku");
        AllowAnonymous();
    }

    public async override Task HandleAsync(RemoveSkuRequest req, CancellationToken ct)
    {
        //var SkuIds = req.Sku.Select(x => x.Id);

        //var existingSku = await _dbContext
        //    .Catalogue
        //    .Where(x => SkuIds.Contains(x.Id))
        //    .ToArrayAsync() ?? Array.Empty<CatalogueEntity>();

        //await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class RemoveSkuRequest
{
    public ProductDto[] Sku { get; set; } = Array.Empty<ProductDto>();
}


