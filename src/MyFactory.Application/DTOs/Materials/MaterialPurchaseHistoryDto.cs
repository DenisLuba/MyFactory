namespace MyFactory.Application.DTOs.Materials;

public sealed record MaterialPurchaseHistoryDto
{
    public Guid SupplierId { get; init; }
    public string SupplierName { get; init; } = default!;
    public decimal Qty { get; init; }
    public decimal UnitPrice { get; init; }
    public DateTime PurchaseDate { get; init; }
}
