namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptLineUpsertResponse(
    Guid ReceiptId,
    MaterialReceiptLineResponse Line,
    MaterialReceiptStatus Status
);
