using MyFactory.MauiClient.Models.ProductionOrders;

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderDetailsResponse(
    Guid Id,
    string ProductionOrderNumber,
    Guid SalesOrderItemId,
    Guid DepartmentId,
    int QtyPlanned,
    int QtyCut,
    int QtySewn,
    int QtyPacked,
    int QtyFinished,
    ProductionOrderStatus Status);
