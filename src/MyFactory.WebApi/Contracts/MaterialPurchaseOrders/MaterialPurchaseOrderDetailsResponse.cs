using MyFactory.Domain.Entities.Materials;

namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderDetailsResponse(
    Guid Id,
    Guid SupplierId,
    string SupplierName,
    DateTime OrderDate,
    PurchaseOrderStatus Status,
    IReadOnlyList<MaterialPurchaseOrderItemResponse> Items);
