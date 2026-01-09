using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.Suppliers;

public sealed record SupplierPurchaseHistoryDto
{
    public Guid OrderId { get; init; }
    public string MaterialType { get; init; } = default!;
    public string MaterialName { get; init; } = default!;
    public decimal Qty { get; init; }
    public decimal UnitPrice { get; init; }
    public DateTime Date { get; init; }
    public PurchaseOrderStatus Status { get; init; }
}
