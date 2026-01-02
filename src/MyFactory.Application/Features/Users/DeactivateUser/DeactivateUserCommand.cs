using MediatR;

namespace MyFactory.Application.Features.Users.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId) : IRequest;
