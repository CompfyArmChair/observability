using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Data.Models;
using SalesApi.Endpoints.Dtos;

namespace SalesApi.Endpoints;

public class GetProductsEndpoint : Endpoint<EmptyRequest, GetProductsResponse, GetProductsMapper>
{
    private readonly SalesDbContext _dbContext;

    public GetProductsEndpoint(SalesDbContext dbContext) =>
        _dbContext = dbContext;

    public override void Configure()
    {
        Get("/Price/Products");
        AllowAnonymous();
    }

    public async override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var products = await _dbContext
            .Products
            .ToArrayAsync() ?? Array.Empty<ProductEntity>();

        var response = Map.FromEntity(products);

        await SendAsync(response);
    }
}

public class GetProductsResponse
{
    public ProductDto[] Products { get; set; } = Array.Empty<ProductDto>();
}

public class GetProductsMapper : Mapper<EmptyRequest, GetProductsResponse, IEnumerable<ProductEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public GetProductsMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductEntity, ProductDto>();
            cfg.CreateMap<IEnumerable<ProductEntity>, GetProductsResponse>()
                .ForMember(dest => dest.Products,
                           opt => opt.MapFrom(src => src));
        });

        _mapper = configuration.CreateMapper();
    }

    public override GetProductsResponse FromEntity(IEnumerable<ProductEntity> productEntities) =>
        _mapper.Map<GetProductsResponse>(productEntities);
}

