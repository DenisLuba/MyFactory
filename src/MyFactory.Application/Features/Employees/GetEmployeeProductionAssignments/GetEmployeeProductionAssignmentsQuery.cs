using MediatR;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Employees.GetEmployeeProductionAssignments;

public sealed record GetEmployeeProductionAssignmentsQuery(
    Guid EmployeeId
) : IRequest<IReadOnlyList<EmployeeProductionAssignmentDto>>;