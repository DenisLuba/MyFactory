using MediatR;

namespace MyFactory.Application.Features.Departments.ActivateDepartments;

public sealed record ActivateDepartmentCommand(Guid DepartmentId) : IRequest;

