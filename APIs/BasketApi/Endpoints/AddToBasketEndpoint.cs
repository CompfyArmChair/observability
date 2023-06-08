using AutoMapper;
using BasketApi.Data;
using BasketApi.Data.Models;
using BasketApi.Endpoints.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Endpoints;

public class AddToBasketEndpoint : Endpoint<AddToBasketRequest, EmptyResponse, AddToBasketBasketMapper>
{
    private readonly BasketDbContext _dbContext;

    public AddToBasketEndpoint(BasketDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Basket/{BasketId}/Product");
        AllowAnonymous();
    }

    public async override Task HandleAsync(AddToBasketRequest req, CancellationToken ct)
    {
        var newProduct = Map.ToEntity(req);

        var basket = await _dbContext
            .Baskets
            .Include(x => x.Products)
            .SingleAsync(x => x.Id == req.BasketId);

        var existingProduct = basket
            .Products
            .SingleOrDefault(x => x.Sku == req.Product.Sku);

        if (existingProduct is null)
            basket.Products.Add(newProduct);
        else
            existingProduct.Quantity += req.Product.Quantity;

        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}


public class AddToBasketRequest
{
    public int BasketId { get; set; }

    [FromBody]
    public ProductDto Product { get; set; }
}

public class AddToBasketBasketMapper : Mapper<AddToBasketRequest, EmptyResponse, ProductEntity>
{
    private readonly AutoMapper.IMapper _mapper;
    public AddToBasketBasketMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ProductDto, ProductEntity>();

            cfg.CreateMap<AddToBasketRequest, ProductEntity>()
                .ConstructUsing((src, ctx) => ctx.Mapper.Map<ProductEntity>(src.Product));
        });

        _mapper = configuration.CreateMapper();
    }

    public override ProductEntity ToEntity(AddToBasketRequest req) =>
        _mapper.Map<ProductEntity>(req);
}

