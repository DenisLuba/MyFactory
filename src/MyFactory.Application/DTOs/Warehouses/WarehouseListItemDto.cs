namespace MyFactory.Application.DTOs.Warehouses;

using MyFactory.Domain.Entities.Inventory;

public sealed record WarehouseListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public WarehouseType Type { get; init; }
    public bool IsActive { get; init; }
}
