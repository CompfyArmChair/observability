using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.ServiceBus.Commands;
using ShopWebsite.Shared;

namespace ShopWebsite.Server.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly HttpClient _httpClient;
		private readonly ISendEndpointProvider _sendEndpointProvider;
		private readonly string _basketApiBaseUrl = "http://basketApi";

        public BasketController(HttpClient httpClient, ISendEndpointProvider sendEndpointProvider)
        {
            _httpClient = httpClient;
			_sendEndpointProvider = sendEndpointProvider;
		}

        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> Get([FromRoute] int id)
        {
			var response = await _httpClient.GetAsync($"{_basketApiBaseUrl}/Basket/{id}");
			var basketResponse = await response.Content.ReadFromJsonAsync<GetBasketResponse>();            
			return Ok(basketResponse.Basket);
        }

        [HttpPost("{BasketId}/Product")]
        public async Task<IActionResult> Add([FromRoute] int basketId, ProductDto product)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:basketApi"));

			await sendEndpoint.Send(new AddProductToBasketCommand()
            {
                BasketId = basketId,
                Cost = product.Cost,
                Name = product.Name,
                Quantity = product.Quantity,
                Sku = product.Sku,                
			});
			
            return Ok();
        }

        [HttpPost("Remove")]
        public IActionResult Remove(ProductDto product)
        {
            return Ok();
        }
    }
}

public class GetBasketResponse
{
	public BasketDto Basket{ get; set; }
}


