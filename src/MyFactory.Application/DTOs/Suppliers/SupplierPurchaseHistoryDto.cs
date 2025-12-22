namespace MyFactory.Application.DTOs.Suppliers;

public sealed record SupplierPurchaseHistoryDto
{
    public string MaterialType { get; init; } = default!;
    public string MaterialName { get; init; } = default!;
    public decimal Qty { get; init; }
    public decimal UnitPrice { get; init; }
    public DateTime Date { get; init; }
}
