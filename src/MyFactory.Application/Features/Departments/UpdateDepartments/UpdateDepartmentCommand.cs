
using MediatR;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Departments.UpdateDepartment;

public sealed record UpdateDepartmentCommand(
    Guid DepartmentId,
    string Name,
    string Code,
    DepartmentType Type,
    bool IsActive
) : IRequest;