using MediatR;
using MyFactory.Application.DTOs.Departments;

namespace MyFactory.Application.Features.Departments.GetDepartments;

public sealed record GetDepartmentsQuery(bool IncludeInactive = false) : IRequest<IReadOnlyList<DepartmentListItemDto>>;

