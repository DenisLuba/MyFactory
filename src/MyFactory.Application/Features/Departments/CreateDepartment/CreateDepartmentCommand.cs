using MediatR;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Departments.CreateDepartment;

public sealed record CreateDepartmentCommand(
    string Name,
    string Code,
    DepartmentType Type
) : IRequest<Guid>;