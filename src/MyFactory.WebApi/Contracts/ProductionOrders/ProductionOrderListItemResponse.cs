using MyFactory.Domain.Entities.Production;

namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderListItemResponse(
    Guid Id,
    string CustomerName,
    int ProductionOrderNumber,
    int SalesOrderNumber,
    string ProductName,
    decimal QtyPlanned,
    decimal QtyFinished,
    ProductionOrderStatus Status);
