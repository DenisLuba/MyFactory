namespace MyFactory.MauiClient.Models.Reports;

public sealed record MonthlyFinancialReportListItemResponse(
    int Year,
    int Month,
    decimal TotalRevenue,
    decimal PayrollExpenses,
    decimal MaterialExpenses,
    decimal OtherExpenses,
    decimal Profit,
    MonthlyReportStatus Status);
