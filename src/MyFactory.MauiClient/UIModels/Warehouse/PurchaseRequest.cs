namespace MyFactory.MauiClient.UIModels.Warehouse;

// Ведомость на закупку
public record PurchaseRequest(
    string Document,
    string Date,
    string Supplier,
    decimal TotalAmount,
    string Status
);

