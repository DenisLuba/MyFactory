namespace MyFactory.WebApi.Contracts.Purchases;

public record PurchasesConvertToOrderResponse(
    Guid PurchaseId,
    PurchasesStatus Status
);

