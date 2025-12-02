namespace MyFactory.MauiClient.Models.WarehouseMaterials;

public record MaterialReceiptPostResponse(
    Guid Id,
    MaterialReceiptStatus Status,
    DateTime PostedAt
);
