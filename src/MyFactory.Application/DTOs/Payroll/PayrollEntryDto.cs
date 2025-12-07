using System;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Application.DTOs.Payroll;

public sealed record PayrollEntryDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    DateOnly PeriodStart,
    DateOnly PeriodEnd,
    decimal TotalHours,
    decimal AccruedAmount,
    decimal PaidAmount,
    decimal Outstanding)
{
    public static PayrollEntryDto FromEntity(PayrollEntry entry, string employeeName)
    {
        return new PayrollEntryDto(
            entry.Id,
            entry.EmployeeId,
            employeeName,
            entry.PeriodStart,
            entry.PeriodEnd,
            entry.TotalHours,
            entry.AccruedAmount,
            entry.PaidAmount,
            entry.Outstanding);
    }
}
