using System;
using System.Collections.Generic;

namespace MyFactory.WebApi.Contracts.Materials;

public record MaterialCardResponse(
    Guid Id,
    string Code,
    string Name,
    string MaterialType,
    string Unit,
    bool IsActive,
    decimal LastPrice,
    IEnumerable<MaterialPriceHistoryItem> PriceHistory
);
