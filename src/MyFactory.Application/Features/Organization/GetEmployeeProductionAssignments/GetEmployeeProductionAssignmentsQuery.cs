using MediatR;
using MyFactory.Application.DTOs.Organization;

namespace MyFactory.Application.Features.GetEmployeeProductionAssignments;

public sealed record GetEmployeeProductionAssignmentsQuery(
    Guid EmployeeId
) : IRequest<IReadOnlyList<EmployeeProductionAssignmentDto>>;