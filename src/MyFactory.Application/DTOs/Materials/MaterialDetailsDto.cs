namespace MyFactory.Application.DTOs.Materials;

public sealed record MaterialDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string MaterialType { get; init; } = default!;
    public string UnitCode { get; init; } = default!;
    public string? Color { get; init; }
    public decimal TotalQty { get; init; }
    public IReadOnlyList<WarehouseQtyDto> Warehouses { get; init; } = [];
    public IReadOnlyList<MaterialPurchaseHistoryDto> PurchaseHistory { get; init; } = [];
}
