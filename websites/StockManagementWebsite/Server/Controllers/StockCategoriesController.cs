using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Instrumentation;
using Shared.ServiceBus.Commands;
using StockManagementWebsite.Shared;
using StockManagementWebsite.Shared.StockCategories;

namespace ShopWebsite.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockCategoriesController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    private readonly string _catalogueApiBaseUrl = "http://catalogueApi";
    private readonly string _salesApiBaseUrl = "http://salesApi";
    private readonly string _warehouseApiBaseUrl = "http://warehouseApi";

    public StockCategoriesController(HttpClient httpClient, ISendEndpointProvider sendEndpointProvider)
    {
        _httpClient = httpClient;
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StockCategoryDto>>> Get()
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

        var composedStockCategories = new List<StockCategoryDto>();

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
                var composedProduct = new StockCategoryDto(
                    catalogueProduct.Sku,
                    catalogueProduct.Name,
                    salesProduct.Cost,
                    warehouseProduct?.Quantity ?? 0);

                composedStockCategories.Add(composedProduct);
            }
        }

		TelemetryBaggageHandler.AddBaggage("stockmanagementwebsite.categories.count", composedStockCategories.Count);

		return composedStockCategories;
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddStockCategoryDto addStockCategoryDto)
    {		
		TelemetryBaggageHandler.AddBaggageFrom(addStockCategoryDto);
		
        var sendCatalogueApiEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:catalogueApi"));
        var sendSaleApiEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:saleApi"));

        await Task.WhenAll(
            sendCatalogueApiEndpoint.Send(new AddNewCatalogueItemCommand()
            {
                Name = addStockCategoryDto.Name,
                Sku = addStockCategoryDto.Sku
            }),
            sendSaleApiEndpoint.Send(new AddNewSaleItemCommand()
            {            
                Sku = addStockCategoryDto.Sku,
                Cost = addStockCategoryDto.Cost,
            }));

        return Ok();
    }

    [HttpPut]
    public async Task<ActionResult> Edit([FromBody] EditStockCategoryDto editStockCategoryDto)
    {
		TelemetryBaggageHandler.AddBaggageFrom(editStockCategoryDto);

		var sendCatalogueApiEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:catalogueApi"));
        var sendSaleApiEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:saleApi"));

        await Task.WhenAll(
            sendCatalogueApiEndpoint.Send(new EditCatalogueItemCommand() 
            { 
                Sku = editStockCategoryDto.Sku,
                Name = editStockCategoryDto.Name,                
            }),
            sendSaleApiEndpoint.Send(new EditSaleItemCommand() 
            {
                Sku = editStockCategoryDto.Sku,
                Cost = editStockCategoryDto.Cost,
            }));

        return Ok();
    }

    [HttpDelete("{Sku}")]
    public async Task<ActionResult> Delete([FromRoute] string sku)
    {
		TelemetryBaggageHandler.AddBaggage("stockmanagementwebsite.category.sku", sku.ToString());

		var sendCatalogueApiEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:catalogueApi"));
        var sendSaleApiEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:saleApi"));
        
        await Task.WhenAll(
            sendCatalogueApiEndpoint.Send(new DeleteCatalogueItemCommand() { Sku =  sku }),
            sendSaleApiEndpoint.Send(new DeleteSaleItemCommand() { Sku = sku}));

        return Ok();
    }

}

public class ProductResponse
{
    public ProductDto[] Products { get; set; } = default!;
}
