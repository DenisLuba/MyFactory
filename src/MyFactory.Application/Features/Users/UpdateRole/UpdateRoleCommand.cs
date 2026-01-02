using MediatR;

namespace MyFactory.Application.Features.Users.UpdateRole;

public sealed record UpdateRoleCommand(Guid RoleId, string Name) : IRequest;
