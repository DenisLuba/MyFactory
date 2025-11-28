namespace MyFactory.MauiClient.Models.Finance;

public record OverheadResponse(
    Guid ExpenseTypeId,
    decimal Amount,
    string Period);
