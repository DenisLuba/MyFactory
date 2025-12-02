namespace MyFactory.MauiClient.Models.Purchases;

public record PurchaseItemRequest(
    Guid MaterialId,
    string MaterialName,
    double Quantity,
    string Unit,
    decimal Price,
    string? Note);
