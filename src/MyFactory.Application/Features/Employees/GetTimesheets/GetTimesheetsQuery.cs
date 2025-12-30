using MediatR;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Employees.GetTimesheets;

public sealed record GetTimesheetsQuery(
    Guid? EmployeeId,
    Guid? DepartmentId,
    YearMonth Period
) : IRequest<IReadOnlyList<TimesheetListItemDto>>;