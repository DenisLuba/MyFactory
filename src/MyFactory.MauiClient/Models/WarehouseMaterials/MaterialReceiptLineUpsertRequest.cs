namespace MyFactory.MauiClient.Models.WarehouseMaterials;

public record MaterialReceiptLineUpsertRequest(
    Guid MaterialId,
    decimal Quantity,
    string Unit,
    decimal Price
);
