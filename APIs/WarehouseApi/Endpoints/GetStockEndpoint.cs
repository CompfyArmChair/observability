using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Endpoints.Dtos;

namespace WarehouseApi.Endpoints;

public class GetStockEndpoint : Endpoint<GetStockEndpointRequest, GetStockEndpointResponse>
{
	private readonly WarehouseDbContext _dbContext;

	public GetStockEndpoint(WarehouseDbContext dbContext) =>
		_dbContext = dbContext;


	public override void Configure()
	{
		Get("/Stock/{sku}");
		AllowAnonymous();
	}

	public async override Task HandleAsync(GetStockEndpointRequest req, CancellationToken ct)
	{
		var stock = await _dbContext
			.Stock
			.Where(x => x.Sku == req.Sku)
			.ToArrayAsync() ?? Array.Empty<StockEntity>();

		var stockDtos = stock
			.Select(x => new StockDto(x.Id, x.Sku, x.DateOfAddition, x.Status))
			.ToArray() ?? Array.Empty<StockDto>();

        var response = new GetStockEndpointResponse()
		{
			Stock = stockDtos
        };

		await SendAsync(response);
	}
}

public class GetStockEndpointResponse
{
	public StockDto[] Stock { get; set; } = Array.Empty<StockDto>();
}

public class GetStockEndpointRequest
{
    public string Sku { get; set; } = string.Empty;
}