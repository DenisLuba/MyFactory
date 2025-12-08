using System;
using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid RoleId) : IRequest<RoleDto>;
