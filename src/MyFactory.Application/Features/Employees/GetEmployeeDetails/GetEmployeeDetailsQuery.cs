using MediatR;
using MyFactory.Application.DTOs.Employees;

namespace MyFactory.Application.Features.Employees.GetEmployeeDetails;

public sealed record GetEmployeeDetailsQuery(
    Guid EmployeeId
) : IRequest<EmployeeDetailsDto>;