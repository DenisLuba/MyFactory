namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderCardResponse(
    Guid OrderId,
    string OrderNumber,
    string ProductName,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate,
    string Responsible,
    string Status);
