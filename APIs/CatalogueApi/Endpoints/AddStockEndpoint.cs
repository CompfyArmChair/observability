using AutoMapper;
using CatalogueApi.Data;
using CatalogueApi.Data.Models;
using CatalogueApi.Endpoints.Dtos;

namespace CatalogueApi.Endpoints;

public class AddSkuEndpoint : Endpoint<AddSkuRequest, EmptyResponse, AddSkuMapper>
{
    private readonly CatalogueDbContext _dbContext;

    public AddSkuEndpoint(CatalogueDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Sku");
        AllowAnonymous();
    }

    public async override Task HandleAsync(AddSkuRequest req, CancellationToken ct)
    {
        var SkuToUpdate = Map.ToEntity(req);

        _dbContext.Catalogue.AddRange(SkuToUpdate);
 
        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class AddSkuRequest
{
    public ProductDto[] Sku { get; set; } = Array.Empty<ProductDto>();
}

public class AddSkuMapper : Mapper<AddSkuRequest, EmptyResponse, IEnumerable<CatalogueEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public AddSkuMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductDto, CatalogueEntity>();
            cfg.CreateMap<AddSkuRequest, IEnumerable<CatalogueEntity>>()
                .ConstructUsing((src, ctx) => 
                ctx.Mapper.Map<IEnumerable<CatalogueEntity>>(src.Sku));
        });

        _mapper = configuration.CreateMapper();
    }

    public override IEnumerable<CatalogueEntity> ToEntity(AddSkuRequest SkuRequest) =>
        _mapper.Map<IEnumerable<CatalogueEntity>>(SkuRequest);
}

