using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record WarehouseListItemResponse(Guid Id, string Name, WarehouseType Type, bool IsActive);
