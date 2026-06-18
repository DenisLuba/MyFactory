using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

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
