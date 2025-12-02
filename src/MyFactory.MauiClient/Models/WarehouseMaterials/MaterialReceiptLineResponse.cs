namespace MyFactory.MauiClient.Models.WarehouseMaterials;

public record MaterialReceiptLineResponse(
    Guid Id,
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal Amount
);
