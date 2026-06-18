using MediatR;

namespace MyFactory.Application.Features.Users.CreateRole;

public sealed record CreateRoleCommand(string Name) : IRequest<Guid>;
