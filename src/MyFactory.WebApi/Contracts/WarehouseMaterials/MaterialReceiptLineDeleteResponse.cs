namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptLineDeleteResponse(
    Guid ReceiptId,
    Guid LineId,
    MaterialReceiptStatus Status
);
