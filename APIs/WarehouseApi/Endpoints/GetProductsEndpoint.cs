using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Endpoints.Dtos;

namespace WarehouseApi.Endpoints;

public class GetProductsEndpoint : Endpoint<EmptyRequest, GetProductsEndpointResponse>
{
	private readonly WarehouseDbContext _dbContext;

	public GetProductsEndpoint(WarehouseDbContext dbContext) =>
		_dbContext = dbContext;


	public override void Configure()
	{
		Get("/Stock/Products");
		AllowAnonymous();
	}

	public async override Task HandleAsync(EmptyRequest req, CancellationToken ct)
	{
		var stock = await _dbContext
			.Stock
			.ToArrayAsync() ?? Array.Empty<StockEntity>();

		var products = new Dictionary<string, ProductDto>();

		foreach (var stockItem in stock)
		{
			if (products.ContainsKey(stockItem.Sku))
			{
				var existingProduct = products[stockItem.Sku];

				products[stockItem.Sku] = existingProduct
					with
				{ Quantity = existingProduct.Quantity + 1 };
			}
			else
			{
				products.Add(stockItem.Sku, new(stockItem.Sku, 1));
			}

		}

		var response = new GetProductsEndpointResponse()
		{
			Products = products.Values.ToArray()
		};

		await SendAsync(response);
	}
}

public class GetProductsEndpointResponse
{
	public ProductDto[] Products { get; set; } = Array.Empty<ProductDto>();
}

