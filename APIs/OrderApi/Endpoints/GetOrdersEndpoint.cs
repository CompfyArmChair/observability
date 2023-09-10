using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Data.Models;
using OrderApi.Endpoints.Dtos;

namespace OrderApi.Endpoints;

public class GetOrdersEndpoint : Endpoint<EmptyRequest, Response, OrdersMapper>
{
    private readonly OrderDbContext _dbContext;

    public GetOrdersEndpoint(OrderDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Get("/Orders");
        AllowAnonymous();
    }

    public async override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var orders = await _dbContext
            .Orders
            .ToArrayAsync() ?? Array.Empty<OrderEntity>();

        var response = Map.FromEntity(orders);

        await SendAsync(response);
    }
}

public class Response
{
    public OrderDto[] Orders { get; set; } = Array.Empty<OrderDto>();
}

public class OrdersMapper : Mapper<EmptyRequest, Response, IEnumerable<OrderEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public OrdersMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<OrderEntity, OrderDto>();
            cfg.CreateMap<ProductEntity, ProductDto>();
            cfg.CreateMap<IEnumerable<OrderEntity>, Response>()
                .ForMember(dest => dest.Orders,
                           opt => opt.MapFrom(src => src));
        });

        _mapper = configuration.CreateMapper();
    }

    public override Response FromEntity(IEnumerable<OrderEntity> orderEntities) =>
        _mapper.Map<Response>(orderEntities);
}

