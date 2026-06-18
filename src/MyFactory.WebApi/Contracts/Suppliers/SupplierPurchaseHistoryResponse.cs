using MyFactory.Application.DTOs.Suppliers;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.WebApi.Contracts.Suppliers;

public sealed record SupplierPurchaseHistoryResponse(
    Guid OrderId,
    decimal PurchaseNumber,
    DateTime Date,
    PurchaseOrderStatus Status,
    IReadOnlyList<SupplierPurchaseHistoryItemResponse> Items);

public sealed record SupplierPurchaseHistoryItemResponse(
    string MaterialType,
    string MaterialName,
    decimal Qty,
    decimal UnitPrice);