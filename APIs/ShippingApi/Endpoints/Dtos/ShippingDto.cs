namespace ShippingApi.Endpoints.Dtos;

public record ShippingDto(
    int Id, 
    string State,
    float Rate);
