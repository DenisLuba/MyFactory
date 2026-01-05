namespace MyFactory.MauiClient.Models.Finance;

public record EmployeePayrollAccrualDetailsResponse(
    Guid EmployeeId,
    string EmployeeName,
    string PositionName,
    YearMonthResponse Period,
    decimal TotalBaseAmount,
    decimal TotalPremiumAmount,
    decimal TotalAmount,
    decimal PaidAmount,
    decimal RemainingAmount,
    IReadOnlyList<EmployeePayrollAccrualDailyResponse> Days);
