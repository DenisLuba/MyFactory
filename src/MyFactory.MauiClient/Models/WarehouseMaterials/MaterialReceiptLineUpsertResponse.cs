namespace MyFactory.MauiClient.Models.WarehouseMaterials;

public record MaterialReceiptLineUpsertResponse(
    Guid ReceiptId,
    MaterialReceiptLineResponse Line,
    MaterialReceiptStatus Status
);
