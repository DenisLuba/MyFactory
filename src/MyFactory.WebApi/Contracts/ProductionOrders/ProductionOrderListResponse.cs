namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderListResponse(
    Guid OrderId,
    string OrderNumber,
    string ProductName,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate,
    string Status);
