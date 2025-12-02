using System;
using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.UIModels.Reference;

public record WarehouseItem(
    Guid Id,
    string Code,
    string Name,
    WarehouseType Type,
    WarehouseStatus Status
);