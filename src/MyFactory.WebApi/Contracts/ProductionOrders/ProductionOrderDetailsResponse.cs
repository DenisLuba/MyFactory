using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

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
