namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptLineResponse(
    Guid Id,
    Guid MaterialId,
    string MaterialName,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal Amount
);
