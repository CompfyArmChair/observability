using AutoMapper;
using BasketApi.Data;
using BasketApi.Data.Models;
using BasketApi.Endpoints.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BasketApi.Endpoints;

public class GetBasketEndpoint : Endpoint<GetBasketRequest, GetBasketResponse, GetBasketBasketMapper>
{
    private readonly BasketDbContext _dbContext;

    public GetBasketEndpoint(BasketDbContext dbContext) =>
        _dbContext = dbContext;

    public override void Configure()
    {
        Get("/Basket/{BasketId}");
        AllowAnonymous();
    }

    public async override Task HandleAsync(GetBasketRequest req, CancellationToken ct)
    {
        BasketEntity? basket = null;

        if (req.BasketId >= 0)
        {
            basket = await _dbContext
                .Baskets
                .Include(x => x.Products)
                .SingleOrDefaultAsync(x => x.Id == req.BasketId);
        }

        if (basket is null)
        {
            basket = new();
            _dbContext.Baskets.Add(basket);
            await _dbContext.SaveChangesAsync();
        }

        var response = Map.FromEntity(basket);

        await SendAsync(response);
    }
}

public class GetBasketRequest
{
    public int BasketId { get; set; }
}

public class GetBasketResponse
{
    public BasketDto Basket { get; set; } = default!;
}

public class GetBasketBasketMapper : Mapper<EmptyRequest, GetBasketResponse, BasketEntity>
{
    private readonly AutoMapper.IMapper _mapper;
    public GetBasketBasketMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {			
			cfg.CreateMap<ProductEntity, ProductDto>();
            cfg.CreateMap<BasketEntity, BasketDto>();

            cfg.CreateMap<BasketEntity, GetBasketResponse>()
                .ForMember(dest => dest.Basket,
                           opt => opt.MapFrom(src => src));
        });

        _mapper = configuration.CreateMapper();
    }

    public override GetBasketResponse FromEntity(BasketEntity basketEntity) =>
        _mapper.Map<GetBasketResponse>(basketEntity);
}

