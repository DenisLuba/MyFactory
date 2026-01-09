using MyFactory.Domain.Entities.Materials;

namespace MyFactory.WebApi.Contracts.Suppliers;

public sealed record SupplierPurchaseHistoryResponse(
    Guid OrderId,
    string MaterialType,
    string MaterialName,
    decimal Qty,
    decimal UnitPrice,
    DateTime Date,
    PurchaseOrderStatus Status);
