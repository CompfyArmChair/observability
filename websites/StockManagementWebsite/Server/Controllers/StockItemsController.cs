using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Instrumentation;
using Shared.ServiceBus.Commands;
using StockManagementWebsite.Shared.StockItems;

namespace StockManagementWebsite.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockItemsController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    private readonly string _warehouseApiBaseUrl = "http://warehouseApi";

    public StockItemsController(HttpClient httpClient, ISendEndpointProvider sendEndpointProvider)
    {
        _httpClient = httpClient;
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpGet("{sku}")]
    public async Task<ActionResult<IEnumerable<StockItemDto>>> Get([FromRoute] string sku)
    {
		TelemetryBaggageHandler.AddBaggage("stockmanagementwebsite.stock.sku", sku);
		
        var result = await _httpClient.GetAsync($"{_warehouseApiBaseUrl}/v2/Stock/{sku}");
        
        result.EnsureSuccessStatusCode();
        var warehouseProducts = await result.Content.ReadFromJsonAsync<StockResponse>() 
            ?? throw new Exception("failed to get stock info");

        return warehouseProducts.Stock;
    }

    [HttpPost]
    public async Task<ActionResult> Add([FromBody] AddStockItemsDto addStockItemsDto)
    {
		TelemetryBaggageHandler.AddBaggageFrom(addStockItemsDto);

		var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:warehouseApi"));

        await sendEndpoint.Send(new AddNewStockItemsCommand()
        {
            Sku = addStockItemsDto.Sku,
            Quantity = addStockItemsDto.Quantity,
        });
        

        return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
		TelemetryBaggageHandler.AddBaggage("stockmanagementwebsite.stock.id", id);

		var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:warehouseApi"));

        await sendEndpoint.Send(new DeleteStockItemCommand() { Id = id });
            
        return Ok();
    }

}

public class StockResponse
{
    public StockItemDto[] Stock { get; set; } = default!;
}
