namespace MyFactory.MauiClient.UIModels.Warehouse;

public record MaterialStockItem(
    string Material,
    string Warehouse,
    decimal Quantity,
    string Unit,
    decimal AvgPrice,
    decimal TotalAmount
);