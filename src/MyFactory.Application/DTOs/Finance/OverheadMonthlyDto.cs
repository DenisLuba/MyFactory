using System;
using MyFactory.Domain.Entities.Reports;

namespace MyFactory.Application.DTOs.Finance;

public sealed record OverheadMonthlyDto(
    Guid Id,
    int PeriodMonth,
    int PeriodYear,
    Guid ExpenseTypeId,
    string ExpenseTypeName,
    decimal Amount,
    string? Notes)
{
    public static OverheadMonthlyDto FromEntity(OverheadMonthly overhead, string expenseTypeName)
    {
        return new OverheadMonthlyDto(
            overhead.Id,
            overhead.PeriodMonth,
            overhead.PeriodYear,
            overhead.ExpenseTypeId,
            expenseTypeName,
            overhead.Amount,
            overhead.Notes);
    }
}
