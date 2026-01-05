using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record CreateWarehouseRequest(string Name, WarehouseType Type);
