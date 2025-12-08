using System;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid RoleId) : IRequest<RoleDto>;
