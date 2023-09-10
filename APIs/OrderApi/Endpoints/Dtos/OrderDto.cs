using OrderApi.Enums;

namespace OrderApi.Endpoints.Dtos;

public record OrderDto(
    int Id,
    int UserId,
    ProductDto[] Products,
    float Tax,
    Status Status);
