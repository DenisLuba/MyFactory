using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record CreateWarehouseRequest(string Name, WarehouseType Type);
