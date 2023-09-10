using AutoMapper;
using BillingApi.Data;
using BillingApi.Data.Models;
using BillingApi.Endpoints.Dtos;

namespace BillingApi.Endpoints;

public class MakePurchaseEndpoint : Endpoint<MakePurchaseRequest, EmptyResponse, MakePurchaseMapper>
{
    private readonly BillingDbContext _dbContext;

    public MakePurchaseEndpoint(BillingDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Purchases");
        AllowAnonymous();
    }

    public async override Task HandleAsync(MakePurchaseRequest req, CancellationToken ct)
    {
        var purchases = Map.ToEntity(req);

        foreach(var purchase in purchases)
        {
            purchase.Status = Enums.Status.Processing;
        }

        _dbContext.Purchases.AddRange(purchases);
        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class MakePurchaseRequest
{
    public PurchaseDto[] Purchases { get; set; } = Array.Empty<PurchaseDto>();
}

public class MakePurchaseMapper : Mapper<MakePurchaseRequest, EmptyResponse, IEnumerable<PurchaseEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public MakePurchaseMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<PurchaseDto, PurchaseEntity>();
            cfg.CreateMap<MakePurchaseRequest, IEnumerable<PurchaseEntity>>()
                .ConstructUsing((src, ctx) =>
                    ctx.Mapper.Map<IEnumerable<PurchaseEntity>>(src.Purchases));
        });

        _mapper = configuration.CreateMapper();
    }

    public override IEnumerable<PurchaseEntity> ToEntity(MakePurchaseRequest purchaseRequest) =>
        _mapper.Map<IEnumerable<PurchaseEntity>>(purchaseRequest);
}

