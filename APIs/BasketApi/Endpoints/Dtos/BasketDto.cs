namespace BasketApi.Endpoints.Dtos;

public record BasketDto(
    string Id,
    ProductDto[] Products);
