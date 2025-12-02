namespace MyFactory.MauiClient.UIModels.Warehouse;

public record PurchaseRequestLineItem(
    Guid LineId,
    Guid MaterialId,
    string MaterialName,
    double Quantity,
    string Unit,
    decimal Price,
    decimal TotalAmount,
    string? Note
);
