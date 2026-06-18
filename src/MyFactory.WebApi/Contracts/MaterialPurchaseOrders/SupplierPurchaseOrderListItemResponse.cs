using MyFactory.Domain.Entities.Materials;

namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public sealed record SupplierPurchaseOrderListItemResponse(
    Guid Id,
    decimal PurchaseNumber,
    DateTime OrderDate,
    PurchaseOrderStatus Status,
    int ItemsCount,
    decimal TotalAmount);
