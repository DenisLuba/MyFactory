using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFactory.Application.DTOs.Payroll;

public sealed record PayrollSummaryDto(
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TotalHours,
    decimal TotalAccruedAmount,
    decimal TotalPaidAmount,
    decimal TotalOutstandingAmount,
    int EmployeesCount,
    IReadOnlyCollection<PayrollEntryDto> Entries)
{
    public static PayrollSummaryDto FromEntries(DateOnly periodStart, DateOnly periodEnd, IReadOnlyCollection<PayrollEntryDto> entries)
    {
        var totalHours = entries.Sum(entry => entry.TotalHours);
        var totalAccrued = entries.Sum(entry => entry.AccruedAmount);
        var totalPaid = entries.Sum(entry => entry.PaidAmount);
        var totalOutstanding = entries.Sum(entry => entry.Outstanding);

        return new PayrollSummaryDto(
            periodStart,
            periodEnd,
            totalHours,
            totalAccrued,
            totalPaid,
            totalOutstanding,
            entries.Count,
            entries);
    }

    public static PayrollSummaryDto Empty(DateOnly periodStart, DateOnly periodEnd)
    {
        return new PayrollSummaryDto(
            periodStart,
            periodEnd,
            0m,
            0m,
            0m,
            0m,
            0,
            Array.Empty<PayrollEntryDto>());
    }
}
