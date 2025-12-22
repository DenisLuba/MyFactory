namespace MyFactory.Application.DTOs.Suppliers;

public sealed record SupplierListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public bool IsActive { get; init; }
}
