using AutoMapper;
using WarehouseApi.Data;
using WarehouseApi.Data.Models;
using WarehouseApi.Endpoints.Dtos;

namespace WarehouseApi.Endpoints;

public class AddStockEndpoint : Endpoint<AddStockRequest, EmptyResponse, AddStockMapper>
{
    private readonly WarehouseDbContext _dbContext;

    public AddStockEndpoint(WarehouseDbContext dbContext) =>
        _dbContext = dbContext;


    public override void Configure()
    {
        Post("/Stock/Products");
        AllowAnonymous();
    }

    public async override Task HandleAsync(AddStockRequest req, CancellationToken ct)
    {
        var stockToUpdate = Map.ToEntity(req);

        _dbContext.Stock.AddRange(stockToUpdate);
 
        await _dbContext.SaveChangesAsync();

        await SendNoContentAsync();
    }
}

public class AddStockRequest
{
    public StockDto[] Stock { get; set; } = Array.Empty<StockDto>();
}

public class AddStockMapper : Mapper<AddStockRequest, EmptyResponse, IEnumerable<StockEntity>>
{
    private readonly AutoMapper.IMapper _mapper;
    public AddStockMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<StockDto, StockEntity>();
            cfg.CreateMap<AddStockRequest, IEnumerable<StockEntity>>()
                .ConstructUsing((src, ctx) => 
                ctx.Mapper.Map<IEnumerable<StockEntity>>(src.Stock));
        });

        _mapper = configuration.CreateMapper();
    }

    public override IEnumerable<StockEntity> ToEntity(AddStockRequest stockRequest) =>
        _mapper.Map<IEnumerable<StockEntity>>(stockRequest);
}

