namespace SalesApi.Endpoints.Dtos;

public record ProductDto(
    int Id, 
    string Sku, 
    decimal Cost);
