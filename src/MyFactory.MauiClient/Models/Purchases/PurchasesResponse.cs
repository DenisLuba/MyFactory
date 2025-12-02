namespace MyFactory.MauiClient.Models.Purchases;

public record PurchasesResponse(
    Guid PurchaseId,
    string DocumentNumber,
    DateTime CreatedAt,
    decimal TotalAmount,
    string[] ItemsSummary,
    PurchasesStatus Status);
