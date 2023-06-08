using BillingApi.Enums;

namespace BillingApi.Endpoints.Dtos;

public record PurchaseDto(
    int Id,
    int OrderId,
    decimal Amount,
    Status Status);
