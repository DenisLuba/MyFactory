namespace MyFactory.Application.DTOs.Suppliers;

public sealed record SupplierDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }

    public IReadOnlyList<SupplierPurchaseHistoryDto> Purchases { get; init; } = [];
}
