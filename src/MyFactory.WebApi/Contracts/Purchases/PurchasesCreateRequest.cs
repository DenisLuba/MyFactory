namespace MyFactory.WebApi.Contracts.Purchases;

public record PurchasesCreateRequest(
    string DocumentNumber,
    DateTime CreatedAt,
    string WarehouseName,
    Guid? SupplierId,
    string? Comment,
    PurchaseItemRequest[] Items
);

public record PurchaseItemRequest(
    Guid MaterialId,
    string MaterialName,
    double Quantity,
    string Unit,
    decimal Price,
    string? Note
);

