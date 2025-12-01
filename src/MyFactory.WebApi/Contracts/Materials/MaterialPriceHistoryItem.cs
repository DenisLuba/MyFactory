using System;

namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialPriceHistoryItem(
    Guid SupplierId,
    string SupplierName,
    decimal Price,
    DateTime EffectiveFrom
);
