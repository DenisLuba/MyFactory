using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(string Name, string? Description) : IRequest<RoleDto>;
