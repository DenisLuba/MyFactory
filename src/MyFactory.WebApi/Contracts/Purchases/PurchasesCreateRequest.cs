namespace MyFactory.WebApi.Contracts.Purchases;

public record PurchasesCreateRequest(
    Guid SupplierId,
    PurchaseItemRequest[] Items
);

public record PurchaseItemRequest(
    Guid MaterialId,
    double Qty
);

