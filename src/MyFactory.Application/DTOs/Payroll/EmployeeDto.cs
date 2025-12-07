using System;
using MyFactory.Domain.Entities.Employees;

namespace MyFactory.Application.DTOs.Payroll;

public sealed record EmployeeDto(
    Guid Id,
    string FullName,
    string Position,
    int Grade,
    decimal RatePerNormHour,
    decimal PremiumPercent,
    bool IsActive)
{
    public static EmployeeDto FromEntity(Employee employee)
    {
        return new EmployeeDto(
            employee.Id,
            employee.FullName,
            employee.Position,
            employee.Grade,
            employee.RatePerNormHour,
            employee.PremiumPercent,
            employee.IsActive);
    }
}
