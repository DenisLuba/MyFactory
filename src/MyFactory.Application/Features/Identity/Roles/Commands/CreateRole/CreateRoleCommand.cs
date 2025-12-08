using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(string Name, string? Description) : IRequest<RoleDto>;
