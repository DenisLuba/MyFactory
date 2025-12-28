using MediatR;
using MyFactory.Application.DTOs.Organization;

namespace MyFactory.Application.Features.GetEmployeeDetails;

public sealed record GetEmployeeDetailsQuery(
    Guid EmployeeId
) : IRequest<EmployeeDetailsDto>;