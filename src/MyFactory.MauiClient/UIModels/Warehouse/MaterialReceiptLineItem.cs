namespace MyFactory.MauiClient.UIModels.Warehouse;

// Позиция поступления материалов
public record MaterialReceiptLineItem(
    string Material,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal TotalAmount
);