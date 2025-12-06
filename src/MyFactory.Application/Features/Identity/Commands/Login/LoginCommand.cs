using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.Features.Identity.Commands.Login;

public sealed record LoginCommand(string UsernameOrEmail, string Password) : IRequest<AuthResultDto>;
