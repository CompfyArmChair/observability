using BasketApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Endpoints;

public class RemoveFromBasketEndpoint : Endpoint<RemoveFromBasketRequest, EmptyResponse>
{
    private readonly BasketDbContext _dbContext;

    public RemoveFromBasketEndpoint(BasketDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Delete("/Basket/{BasketId}/Product/{Sku}");
        AllowAnonymous();
    }

    public async override Task HandleAsync(RemoveFromBasketRequest req, CancellationToken ct)
    {
        var product = await _dbContext
            .Baskets
            .Include(x => x.Products)
            .Where(x => x.Id == req.BasketId)
            .SelectMany(x => x.Products)
            .SingleAsync(x => x.Sku == req.Sku);

        if (product.Quantity > 1)
            product.Quantity--;
        else
            _dbContext.Remove(product);

        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}


public class RemoveFromBasketRequest
{
    public int BasketId { get; set; }
    public string Sku { get; set; } = string.Empty;
}


