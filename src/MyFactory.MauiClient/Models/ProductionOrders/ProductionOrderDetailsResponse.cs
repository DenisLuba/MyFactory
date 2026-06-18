using MyFactory.MauiClient.Models.ProductionOrders;

namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderDetailsResponse(
    Guid Id,
    int ProductionOrderNumber,
    Guid SalesOrderId,
    Guid SalesOrderItemId,
    Guid ProductId,
    string? ProductName,
    Guid DepartmentId,
    string? DepartmentName,
    decimal QtyPlanned,
    decimal QtyCut,
    decimal QtySewn,
    decimal QtyPacked,
    decimal QtyFinished,
    ProductionOrderStatus Status);
