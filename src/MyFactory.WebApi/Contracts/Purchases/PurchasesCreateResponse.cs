namespace MyFactory.WebApi.Contracts.Purchases;

public record PurchasesCreateResponse(
    Guid PurchaseId,
    PurchasesStatus Status
);

