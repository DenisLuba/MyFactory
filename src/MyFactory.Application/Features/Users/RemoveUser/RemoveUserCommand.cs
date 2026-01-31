using MediatR;

namespace MyFactory.Application.Features.Users.RemoveUser;

public sealed record RemoveUserCommand(Guid UserId) : IRequest;
