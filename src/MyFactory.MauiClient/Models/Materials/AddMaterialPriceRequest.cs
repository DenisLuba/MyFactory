namespace MyFactory.MauiClient.Models.Materials;

public record AddMaterialPriceRequest(
    Guid SupplierId,
    decimal MaterialPrice,
    DateTime EffectiveFrom);
