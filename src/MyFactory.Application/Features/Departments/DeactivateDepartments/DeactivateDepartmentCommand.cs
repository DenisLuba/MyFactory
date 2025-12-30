using MediatR;

namespace MyFactory.Application.Features.Departments.DeactivateDepartments;

public sealed record DeactivateDepartmentCommand(Guid DepartmentId) : IRequest;