using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Data.Models;
using SalesApi.Endpoints.Dtos;

namespace SalesApi.Endpoints;

public class AddProductsEndpoint : Endpoint<AddProductsRequest, EmptyResponse, AddProductsMapper>
{
    private readonly SalesDbContext _dbContext;

    public AddProductsEndpoint(SalesDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Price/Products");
        AllowAnonymous();
    }

    public async override Task HandleAsync(AddProductsRequest req, CancellationToken ct)
    {
        var productsToUpdate = Map.ToEntity(req);

        var productSkus = productsToUpdate.Select(x => x.Sku);

        var existingProducts = await _dbContext
            .Products
            .Where(x => productSkus.Contains(x.Sku))
            .ToArrayAsync() ?? Array.Empty<ProductEntity>();

        foreach (var productUpdate in productsToUpdate)
        {
            var existingProduct = existingProducts.SingleOrDefault(x => x.Sku == productUpdate.Sku);

            if (existingProduct is null)
            {
                _dbContext.Products.Add(productUpdate);
            }
        }

        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class AddProductsRequest
{
    public ProductDto[] Products { get; set; } = Array.Empty<ProductDto>();
}

public class AddProductsMapper : Mapper<AddProductsRequest, EmptyResponse, IEnumerable<ProductEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public AddProductsMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductDto, ProductEntity>();
            cfg.CreateMap<AddProductsRequest, IEnumerable<ProductEntity>>()
                .ConstructUsing((src, ctx) => 
                    ctx.Mapper.Map<IEnumerable<ProductEntity>>(src.Products));
        });

        _mapper = configuration.CreateMapper();
    }

    public override IEnumerable<ProductEntity> ToEntity(AddProductsRequest productRequest) =>
        _mapper.Map<IEnumerable<ProductEntity>>(productRequest);
}

