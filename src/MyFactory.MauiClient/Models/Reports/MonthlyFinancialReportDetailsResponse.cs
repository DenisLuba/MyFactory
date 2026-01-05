namespace MyFactory.MauiClient.Models.Reports;

public sealed record MonthlyFinancialReportDetailsResponse(
    Guid Id,
    int Year,
    int Month,
    decimal TotalRevenue,
    decimal PayrollExpenses,
    decimal MaterialExpenses,
    decimal OtherExpenses,
    decimal Profit,
    MonthlyReportStatus Status,
    DateTime CalculatedAt,
    Guid CreatedBy);
