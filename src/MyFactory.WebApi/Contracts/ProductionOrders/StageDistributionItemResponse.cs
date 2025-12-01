namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record StageDistributionItemResponse(
    string Stage,
    string Employee,
    double Hours,
    int Quantity,
    string Status);
