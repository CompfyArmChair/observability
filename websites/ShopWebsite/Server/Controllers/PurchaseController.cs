using System.Diagnostics;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Instrumentation;
using Shared.ServiceBus.Commands;
using ShopWebsite.Shared;

namespace ShopWebsite.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PurchaseController : ControllerBase
    {
		private readonly ISendEndpointProvider _sendEndpointProvider;
		private readonly IMapper _mapper;

		public PurchaseController(ISendEndpointProvider sendEndpointProvider)
		{
			_sendEndpointProvider = sendEndpointProvider;

			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<ProductDto, ProductCommandDto>();
				cfg.CreateMap<BasketDto, BasketCommandDto>()
				   .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));
				cfg.CreateMap<PurchaseDto, MakePurchaseCommand>();
			});

			_mapper = configuration.CreateMapper();

		}

		[HttpPost("Checkout")]
		public async Task<IActionResult> Checkout(PurchaseDto purchase)
		{
            TelemetryBaggageHandler.AddBaggageFrom(purchase.Basket);

            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:orderApi"));

            var command = _mapper.Map<MakePurchaseCommand>(purchase);

            await sendEndpoint.Send(command);

            return Ok();			
		}
    }
}
