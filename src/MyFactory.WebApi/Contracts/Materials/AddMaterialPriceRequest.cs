namespace MyFactory.WebApi.Contracts.Materials;

public record AddMaterialPriceRequest(
    Guid SupplierId,
    decimal MaterialPrice,
    DateTime EffectiveFrom
);