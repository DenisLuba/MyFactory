namespace MyFactory.WebApi.Contracts.Purchases;

public record PurchasesResponse(
    Guid PurchaseId,
    DateTime CreatedAt,
    PurchasesStatus Status,
    PurchaseResponseItem[] Items
);

public record PurchaseResponseItem(Guid MaterialId, double Qty);
