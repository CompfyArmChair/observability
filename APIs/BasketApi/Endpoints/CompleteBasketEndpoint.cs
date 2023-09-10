using BasketApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Endpoints;

public class CompleteBasketEndpoint : Endpoint<CompleteBasketRequest, EmptyResponse>
{
    private readonly BasketDbContext _dbContext;

    public CompleteBasketEndpoint(BasketDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Get("/Basket/{BasketId}/Command/Complete");
        AllowAnonymous();
    }

    public async override Task HandleAsync(CompleteBasketRequest req, CancellationToken ct)
    {
        var products = await _dbContext
            .Baskets
            .Include(x => x.Products)
            .Where(x => x.Id == req.UserId)
            .ToArrayAsync();

        if (products.Any())
        {
            _dbContext.RemoveRange(products);
            await _dbContext.SaveChangesAsync();
        }

        await SendNoContentAsync();
    }
}


public class CompleteBasketRequest
{
    public int UserId { get; set; }
}


