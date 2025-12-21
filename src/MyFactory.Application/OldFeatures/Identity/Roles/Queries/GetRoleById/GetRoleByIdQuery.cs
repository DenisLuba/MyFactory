using System;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid RoleId) : IRequest<RoleDto>;
