using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.DTOs.Reports;

public sealed record MonthlyFinancialReportDetailsDto(
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
    Guid CreatedBy
);
