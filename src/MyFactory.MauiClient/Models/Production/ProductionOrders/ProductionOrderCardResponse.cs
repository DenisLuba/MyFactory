namespace MyFactory.MauiClient.Models.Production.ProductionOrders;

public record ProductionOrderCardResponse(
    Guid OrderId,
    string OrderNumber,
    string ProductName,
    int Quantity,
    DateTime StartDate,
    DateTime EndDate,
    string Responsible,
    string Status);
