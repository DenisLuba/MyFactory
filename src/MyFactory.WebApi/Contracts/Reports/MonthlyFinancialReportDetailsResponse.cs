using MyFactory.Domain.Entities.Finance;

namespace MyFactory.WebApi.Contracts.Reports;

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
