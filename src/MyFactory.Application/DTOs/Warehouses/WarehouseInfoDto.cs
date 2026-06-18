using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.DTOs.Warehouses;

public sealed class WarehouseInfoDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public WarehouseType Type { get; init; }
}