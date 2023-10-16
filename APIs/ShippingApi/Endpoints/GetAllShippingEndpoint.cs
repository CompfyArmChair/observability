using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShippingApi.Data;
using ShippingApi.Data.Models;
using ShippingApi.Endpoints.Dtos;

namespace ShippingApi.Endpoints;

public class GetAllShippingEndpoint : Endpoint<EmptyRequest, Response, ShippingMapper>
{
    private readonly ShippingDbContext _dbContext;

    public GetAllShippingEndpoint(ShippingDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Get("/v2/Shippings");
        AllowAnonymous();
    }

    public async override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var shippings = await _dbContext
            .Shipments            
            .ToArrayAsync() ?? Array.Empty<ShipmentEntity>();

        var response = Map.FromEntity(shippings);

        await SendAsync(response);
    }
}

public class Response
{
    public ShippingDto[] Shippings { get; set; } = Array.Empty<ShippingDto>();
}

public class ShippingMapper : Mapper<EmptyRequest, Response, IEnumerable<ShipmentEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public ShippingMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ShipmentEntity, ShippingDto>();
            cfg.CreateMap<IEnumerable<ShipmentEntity>, Response>()
                .ForMember(dest => dest.Shippings,
                           opt => opt.MapFrom(src => src));
        });

        _mapper = configuration.CreateMapper();
    }

    public override Response FromEntity(IEnumerable<ShipmentEntity> shippingEntities) =>
        _mapper.Map<Response>(shippingEntities);
}

