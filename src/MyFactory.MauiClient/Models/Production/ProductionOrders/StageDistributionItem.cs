namespace MyFactory.MauiClient.Models.Production.ProductionOrders;

public record StageDistributionItem(
    string Stage,
    string Employee,
    double Hours,
    int Quantity,
    string Status);
