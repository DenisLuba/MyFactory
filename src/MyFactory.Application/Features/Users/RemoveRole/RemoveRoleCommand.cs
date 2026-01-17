using MediatR;

namespace MyFactory.Application.Features.Users.DeactivateRole;

public sealed record RemoveRoleCommand(Guid RoleId) : IRequest;
