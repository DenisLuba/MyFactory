namespace MyFactory.MauiClient.Models.Inventory;

public record ReceiptItem(
    Guid MaterialId,
    double Quantity,
    decimal UnitPrice,
    string? BatchNumber = null);
