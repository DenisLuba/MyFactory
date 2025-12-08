using System.Collections.Generic;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Queries.GetRoles;

public sealed record GetRolesQuery() : IRequest<IReadOnlyCollection<RoleDto>>;
