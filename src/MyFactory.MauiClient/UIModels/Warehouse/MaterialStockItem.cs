namespace MyFactory.MauiClient.UIModels.Warehouse;

// ѕозици€ остатка материалов на складе
public record MaterialStockItem(
    string Material,
    string Warehouse,
    decimal Quantity,
    string Unit,
    decimal AvgPrice,
    decimal TotalAmount
);