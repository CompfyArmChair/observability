using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Data.Models;
using SalesApi.Endpoints.Dtos;

namespace SalesApi.Endpoints;

public class RemoveProductsEndpoint : Endpoint<RemoveProductsRequest, EmptyResponse>
{
	private readonly SalesDbContext _dbContext;

	public RemoveProductsEndpoint(SalesDbContext dbContext) =>
		_dbContext = dbContext;


	public override void Configure()
	{
		Post("/Price/Products/Command/RemoveProducts");
		AllowAnonymous();
	}

	public async override Task HandleAsync(RemoveProductsRequest req, CancellationToken ct)
	{
		var productSkus = req.Products.Select(x => x.Sku);

		var existingProducts = await _dbContext
			.Products
			.Where(x => productSkus.Contains(x.Sku))
			.ToArrayAsync() ?? Array.Empty<ProductEntity>();

		foreach (var productDto in req.Products)
		{
			var existingProduct = existingProducts.SingleOrDefault(x => x.Sku == productDto.Sku);

			if (existingProduct is not null)
			{
				_dbContext.Products.Remove(existingProduct);
			}
		}

		await _dbContext.SaveChangesAsync();

		await SendNoContentAsync();
	}
}

public class RemoveProductsRequest
{
	public ProductDto[] Products { get; set; } = Array.Empty<ProductDto>();
}


