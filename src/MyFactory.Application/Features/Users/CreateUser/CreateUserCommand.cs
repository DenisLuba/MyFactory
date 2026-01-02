using MediatR;

namespace MyFactory.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(string Username, string Password, Guid RoleId) : IRequest<Guid>;
