namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderUpdateRequest(
    string ProductName,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate,
    string Responsible,
    string Status);
