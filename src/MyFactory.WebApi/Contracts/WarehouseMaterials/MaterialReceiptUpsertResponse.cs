namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptUpsertResponse(
    Guid Id,
    MaterialReceiptStatus Status
);
