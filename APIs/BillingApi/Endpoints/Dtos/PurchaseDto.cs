using BillingApi.Enums;

namespace BillingApi.Endpoints.Dtos;

public record PurchaseDto(
    int Id,
    Guid OrderId,
    decimal Amount,
    Status Status);
