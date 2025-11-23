namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialPriceHistoryResponse(
    Guid MaterialId,
    Guid SupplierId,
    decimal Price,
    DateTime EffectiveFrom
);