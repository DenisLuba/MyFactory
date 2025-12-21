using System;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(Guid RoleId, string Name, string? Description) : IRequest<RoleDto>;
