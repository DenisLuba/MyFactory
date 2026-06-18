using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record UpdateWarehouseRequest(string Name, WarehouseType Type);
