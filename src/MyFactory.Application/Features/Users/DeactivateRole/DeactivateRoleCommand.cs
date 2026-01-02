using MediatR;

namespace MyFactory.Application.Features.Users.DeactivateRole;

public sealed record DeactivateRoleCommand(Guid RoleId) : IRequest;
