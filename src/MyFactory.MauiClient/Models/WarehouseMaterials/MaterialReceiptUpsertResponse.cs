namespace MyFactory.MauiClient.Models.WarehouseMaterials;

public record MaterialReceiptUpsertResponse(
    Guid Id,
    MaterialReceiptStatus Status
);
