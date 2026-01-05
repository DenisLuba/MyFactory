using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.WebApi.Contracts.Warehouses;

public sealed record WarehouseInfoResponse(Guid Id, string Name, WarehouseType Type);
