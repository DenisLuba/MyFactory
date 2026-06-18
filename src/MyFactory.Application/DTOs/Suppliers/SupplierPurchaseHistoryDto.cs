using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.Suppliers;

public sealed record SupplierPurchaseHistoryDto
{
    public Guid OrderId { get; init; }
    public decimal PurchaseNumber { get; init; }
    public DateTime Date { get; init; }
    public PurchaseOrderStatus Status { get; init; }
    public IReadOnlyList<SupplierPurchaseHistoryItemDto> Items { get; init; } = [];
}

public sealed record SupplierPurchaseHistoryItemDto(
    string MaterialType,
    string MaterialName,
    decimal Qty,
    decimal UnitPrice);
