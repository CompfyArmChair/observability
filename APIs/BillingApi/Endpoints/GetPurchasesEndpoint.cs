using AutoMapper;
using BillingApi.Data;
using BillingApi.Data.Models;
using BillingApi.Endpoints.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BillingApi.Endpoints;

public class GetPurchasesEndpoint : Endpoint<EmptyRequest, GetPurchasesResponse, GetPurchasesMapper>
{
    private readonly BillingDbContext _dbContext;

    public GetPurchasesEndpoint(BillingDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Get("/Purchases");
        AllowAnonymous();
    }

    public async override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var purchases = await _dbContext
            .Purchases
            .ToArrayAsync() ?? Array.Empty<PurchaseEntity>();

        var response = Map.FromEntity(purchases);

        await SendAsync(response);
    }
}

public class GetPurchasesResponse
{
    public PurchaseDto[] Purchases { get; set; } = Array.Empty<PurchaseDto>();
}

public class GetPurchasesMapper : Mapper<EmptyRequest, GetPurchasesResponse, IEnumerable<PurchaseEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public GetPurchasesMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PurchaseEntity, PurchaseDto>();
            cfg.CreateMap<IEnumerable<PurchaseEntity>, GetPurchasesResponse>()
                .ForMember(dest => dest.Purchases,
                           opt => opt.MapFrom(src => src));
        });

        _mapper = configuration.CreateMapper();
    }

    public override GetPurchasesResponse FromEntity(IEnumerable<PurchaseEntity> productEntities) =>
        _mapper.Map<GetPurchasesResponse>(productEntities);
}

