using MediatR;
using MyFactory.Application.Common.ValueObjects;
using MyFactory.Application.DTOs.Organization;

namespace MyFactory.Application.Features.GetEmployeeTimesheetDetails;

public sealed record GetEmployeeTimesheetDetailsQuery(
    Guid EmployeeId,
    YearMonth Period
) : IRequest<IReadOnlyList<EmployeeTimesheetEntryDto>>;