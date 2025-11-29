namespace MyFactory.MauiClient.UIModels.Production;

// Производственный заказ
public record ProductionOrder(
    string OrderId,
    string ProductName,
    int Quantity,
    string Status
);

