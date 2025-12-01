using System;

namespace MyFactory.MauiClient.Models.Materials;

public record MaterialListResponse(
    Guid Id,
    string Code,
    string Name,
    string MaterialType,
    string Unit,
    bool IsActive,
    decimal LastPrice
);
