using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record WarehouseInfoResponse(Guid Id, string Name, WarehouseType Type);
