namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptPostResponse(
    Guid Id,
    MaterialReceiptStatus Status,
    DateTime PostedAt
);
