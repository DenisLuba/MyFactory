using System;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid RoleId) : IRequest<RoleDto>;
