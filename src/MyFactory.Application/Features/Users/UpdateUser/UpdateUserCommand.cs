using MediatR;

namespace MyFactory.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId, Guid RoleId, bool IsActive) : IRequest;
