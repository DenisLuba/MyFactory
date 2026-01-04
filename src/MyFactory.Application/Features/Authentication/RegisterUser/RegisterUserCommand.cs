using MediatR;
using MyFactory.Application.DTOs.Authentication;

namespace MyFactory.Application.Features.Authentication.RegisterUser;

public sealed record RegisterUserCommand(string Username, string Password, Guid RoleId) : IRequest<RegisterUserDto>;
