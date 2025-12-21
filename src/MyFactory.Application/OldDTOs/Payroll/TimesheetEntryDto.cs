using System;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Application.OldDTOs.Payroll;

public sealed record TimesheetEntryDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    DateOnly WorkDate,
    decimal HoursWorked,
    Guid? ProductionOrderId,
    string Status)
{
    public static TimesheetEntryDto FromEntity(TimesheetEntry entry, string employeeName)
    {
        return new TimesheetEntryDto(
            entry.Id,
            entry.EmployeeId,
            employeeName,
            entry.WorkDate,
            entry.HoursWorked,
            entry.ProductionOrderId,
            entry.Status);
    }
}
