namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptLineUpsertRequest(
    Guid MaterialId,
    decimal Quantity,
    string Unit,
    decimal Price
);
