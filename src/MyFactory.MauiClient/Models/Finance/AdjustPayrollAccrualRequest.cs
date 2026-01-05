namespace MyFactory.MauiClient.Models.Finance;

public record AdjustPayrollAccrualRequest(
    decimal BaseAmount,
    decimal PremiumAmount,
    string Reason);
