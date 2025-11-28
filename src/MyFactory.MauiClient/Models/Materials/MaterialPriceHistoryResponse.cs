namespace MyFactory.MauiClient.Models.Materials;

public record MaterialPriceHistoryResponse(
    Guid MaterialId,
    Guid SupplierId,
    decimal Price,
    DateTime EffectiveFrom);
