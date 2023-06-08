using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CatalogueApi.Data;
using CatalogueApi.Data.Models;
using CatalogueApi.Endpoints.Dtos;

namespace CatalogueApi.Endpoints;

public class GetProductsEndpoint : Endpoint<EmptyRequest, GetProductsEndpointResponse, GetAllProductsMapper>
{
    private readonly CatalogueDbContext _dbContext;

    public GetProductsEndpoint(CatalogueDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Get("/Catalogue/Products");
        AllowAnonymous();
    }

    public async override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var products = await _dbContext
            .Catalogue
			.ToArrayAsync() ?? Array.Empty<CatalogueEntity>();

        var response = Map.FromEntity(products);

        await SendAsync(response);
    }
}

public class GetProductsEndpointResponse
{
    public ProductDto[] Products { get; set; } = Array.Empty<ProductDto>();
}

public class GetAllProductsMapper : Mapper<EmptyRequest, GetProductsEndpointResponse, IEnumerable<CatalogueEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public GetAllProductsMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
			cfg.CreateMap<CatalogueEntity, ProductDto>();
			cfg.CreateMap<IEnumerable<CatalogueEntity>, GetProductsEndpointResponse>()
				.ForMember(dest => dest.Products,
						   opt => opt.MapFrom(src => src));
		});

        _mapper = configuration.CreateMapper();
    }

    public override GetProductsEndpointResponse FromEntity(IEnumerable<CatalogueEntity> SkuEntities) =>
        _mapper.Map<GetProductsEndpointResponse>(SkuEntities);
}

