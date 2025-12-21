using MediatR;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Commands.Login;

public sealed record LoginCommand(string UsernameOrEmail, string Password) : IRequest<AuthResultDto>;
