﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Endpoints.Dtos;
using WarehouseApi.Enums;

namespace WarehouseApi.Endpoints;

public class GetAvailableProductsEndpoint : Endpoint<EmptyRequest, GetAvailableProductsEndpointResponse>
{
	private readonly WarehouseDbContext _dbContext;

	public GetAvailableProductsEndpoint(WarehouseDbContext dbContext) =>
		_dbContext = dbContext;


	public override void Configure()
	{
		Get("/v2/Stock/Products");
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
			var isAvailable = stockItem.Status is Status.Available;

			if (products.ContainsKey(stockItem.Sku))
			{
				var existingProduct = products[stockItem.Sku];

				if (isAvailable)
				{
					products[stockItem.Sku] = existingProduct
						with
					{ Quantity = existingProduct.Quantity + 1 };
				}
			}
			else
			{
				products.Add(stockItem.Sku, new(stockItem.Sku, isAvailable ? 1 : 0));
			}

		}

		var response = new GetAvailableProductsEndpointResponse()
		{
			Products = products.Values.ToArray()
		};

		await SendAsync(response);
	}
}

public class GetAvailableProductsEndpointResponse
{
	public ProductDto[] Products { get; set; } = Array.Empty<ProductDto>();
}

