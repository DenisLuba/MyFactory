using MyFactory.Domain.Entities.Materials;

namespace MyFactory.WebApi.Contracts.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderItemResponse(
    Guid Id,
    Guid MaterialId,
    string MaterialName,
    string UnitCode,
    decimal Qty,
    decimal UnitPrice);
