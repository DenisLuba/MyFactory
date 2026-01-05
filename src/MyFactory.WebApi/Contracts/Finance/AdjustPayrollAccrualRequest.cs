namespace MyFactory.WebApi.Contracts.Finance;

public record AdjustPayrollAccrualRequest(
    decimal BaseAmount,
    decimal PremiumAmount,
    string Reason);
