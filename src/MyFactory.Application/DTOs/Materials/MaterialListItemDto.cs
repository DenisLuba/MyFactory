namespace MyFactory.Application.DTOs.Materials;

public sealed record MaterialListItemDto
{
    public Guid Id { get; init; }
    public string MaterialType { get; init; } = default!;
    public string Name { get; init; } = default!;
    public decimal TotalQty { get; init; }
    public string UnitCode { get; init; } = default!;
}
