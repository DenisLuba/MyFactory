namespace MyFactory.WebApi.Contracts.WarehouseMaterials;

public record MaterialReceiptUpsertRequest(
    string DocumentNumber,
    DateTime DocumentDate,
    string SupplierName,
    string WarehouseName,
    decimal TotalAmount,
    string? Comment
);
