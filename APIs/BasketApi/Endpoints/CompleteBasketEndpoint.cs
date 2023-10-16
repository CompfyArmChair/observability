using BasketApi.Data;
using BasketApi.Instrumentation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace BasketApi.Endpoints;

public class CompleteBasketEndpoint : Endpoint<CompleteBasketRequest, EmptyResponse>
{
    private readonly BasketDbContext _dbContext;
	private readonly OtelMeters _meter;

	public CompleteBasketEndpoint(BasketDbContext dbContext, OtelMeters meter)
    {
        _dbContext = dbContext;
        _meter = meter;
	}

    public override void Configure()
    {
        Get("/v2/Basket/{BasketId}/Command/Complete");
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
        _meter.BasketCompleted();

		await SendNoContentAsync();
    }
}


public class CompleteBasketRequest
{
    public int UserId { get; set; }
}


