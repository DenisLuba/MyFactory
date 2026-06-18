using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.DTOs.Reports;

public sealed record MonthlyFinancialReportListItemDto(
    int Year,
    int Month,
    decimal TotalRevenue,
    decimal PayrollExpenses,
    decimal MaterialExpenses,
    decimal OtherExpenses,
    decimal Profit,
    MonthlyReportStatus Status
);
