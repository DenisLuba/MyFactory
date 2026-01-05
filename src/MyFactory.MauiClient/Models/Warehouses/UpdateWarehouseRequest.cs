using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record UpdateWarehouseRequest(string Name, WarehouseType Type);
