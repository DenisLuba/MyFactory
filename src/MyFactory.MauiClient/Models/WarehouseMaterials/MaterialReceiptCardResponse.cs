namespace MyFactory.MauiClient.Models.WarehouseMaterials;

public record MaterialReceiptCardResponse(
    Guid Id,
    string DocumentNumber,
    DateTime DocumentDate,
    string SupplierName,
    string WarehouseName,
    decimal TotalAmount,
    MaterialReceiptStatus Status,
    string? Comment
);
