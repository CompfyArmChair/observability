using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Shared.Instrumentation;
using ShopWebsite.Shared;

namespace ShopWebsite.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
	private readonly HttpClient _httpClient;

	private readonly string _catalogueApiBaseUrl = "http://catalogueApi";
	private readonly string _salesApiBaseUrl = "http://salesApi";
	private readonly string _warehouseApiBaseUrl = "http://warehouseApi";

	public ProductsController(HttpClient httpClient) => _httpClient = httpClient;

	[HttpGet]
	public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
	{
		var catalogueProductTask = _httpClient.GetAsync($"{_catalogueApiBaseUrl}/v2/Catalogue/Products");
		var salesProductTask = _httpClient.GetAsync($"{_salesApiBaseUrl}/v2/Price/Products");
		var warehouseProductTask = _httpClient.GetAsync($"{_warehouseApiBaseUrl}/v2/Stock/Products");

		var results = await Task.WhenAll(catalogueProductTask, salesProductTask, warehouseProductTask);

		var failedResults = results.Where(x => !x.IsSuccessStatusCode);

		if (failedResults.Any())
		{
			throw new AggregateException(string.Concat(failedResults.Select(x => x.StatusCode)));
		}

		var catalogueProducts = await catalogueProductTask.Result.Content.ReadFromJsonAsync<ProductResponse>();
		var salesProducts = await salesProductTask.Result.Content.ReadFromJsonAsync<ProductResponse>();
		var warehouseProducts = await warehouseProductTask.Result.Content.ReadFromJsonAsync<ProductResponse>();

		var composedProducts = new List<ProductDto>();

		if (catalogueProducts is null || salesProducts is null || warehouseProducts is null)
		{
			throw new Exception("failed to get all product info");
		}

		foreach (var catalogueProduct in catalogueProducts.Products)
		{
			var salesProduct = salesProducts.Products.SingleOrDefault(x => x.Sku == catalogueProduct.Sku);
			var warehouseProduct = warehouseProducts.Products.SingleOrDefault(x => x.Sku == catalogueProduct.Sku);

			if (salesProduct is not null)
			{
				var composedProduct = new ProductDto(
					catalogueProduct.Sku,
					catalogueProduct.Name,
					salesProduct.Cost,
					warehouseProduct?.Quantity ?? 0);

				composedProducts.Add(composedProduct);
			}
		}

		TelemetryBaggageHandler.AddBaggage("shopwebsite.products.count", composedProducts.Count);
		return composedProducts;
	}
}

public class ProductResponse
{
	public ProductDto[] Products { get; set; } = default!;
}
