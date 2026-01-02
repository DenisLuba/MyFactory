using MediatR;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetRoles;

public sealed record GetRolesQuery : IRequest<IReadOnlyList<RoleDto>>;
