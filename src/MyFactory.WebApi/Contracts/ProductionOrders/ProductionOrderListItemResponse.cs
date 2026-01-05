using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderListItemResponse(
    Guid Id,
    string ProductionOrderNumber,
    string SalesOrderNumber,
    string ProductName,
    int QtyPlanned,
    int QtyFinished,
    ProductionOrderStatus Status);
