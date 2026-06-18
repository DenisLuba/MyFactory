using MediatR;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Employees.GetEmployeeTimesheetDetails;

public sealed record GetEmployeeTimesheetDetailsQuery(
    Guid EmployeeId,
    YearMonth Period
) : IRequest<IReadOnlyList<EmployeeTimesheetEntryDto>>;