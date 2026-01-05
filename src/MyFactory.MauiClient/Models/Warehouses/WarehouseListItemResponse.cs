using MyFactory.MauiClient.Models.Warehouses;

namespace MyFactory.MauiClient.Models.Warehouses;

public sealed record WarehouseListItemResponse(Guid Id, string Name, WarehouseType Type, bool IsActive);
