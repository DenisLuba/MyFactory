using MediatR;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.DTOs.Organization;

namespace MyFactory.Application.Features.GetTimesheets;

public sealed record GetTimesheetsQuery(
    Guid? EmployeeId,
    Guid? DepartmentId,
    YearMonth Period
) : IRequest<IReadOnlyList<TimesheetListItemDto>>;