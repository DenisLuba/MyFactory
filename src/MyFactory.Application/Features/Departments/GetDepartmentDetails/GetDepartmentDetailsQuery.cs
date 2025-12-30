using MediatR;
using MyFactory.Application.DTOs.Departments;

namespace MyFactory.Application.Features.Departments.GetDepartmentDetails;

public sealed record GetDepartmentDetailsQuery
(
    Guid DepartmentId
) : IRequest<DepartmentDetailsDto>;