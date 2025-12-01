using System;

namespace MyFactory.MauiClient.Models.Materials;

public record MaterialPriceHistoryItem(
    Guid SupplierId,
    string SupplierName,
    decimal Price,
    DateTime EffectiveFrom
);
