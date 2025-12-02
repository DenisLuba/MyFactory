namespace MyFactory.MauiClient.Models.Purchases;

public record PurchaseRequestDetailResponse(
    Guid PurchaseId,
    string DocumentNumber,
    DateTime CreatedAt,
    string WarehouseName,
    Guid? SupplierId,
    string? Comment,
    decimal TotalAmount,
    PurchasesStatus Status,
    PurchaseRequestLineResponse[] Items
);

public record PurchaseRequestLineResponse(
    Guid LineId,
    Guid MaterialId,
    string MaterialName,
    double Quantity,
    string Unit,
    decimal Price,
    decimal TotalAmount,
    string? Note
);
