using MediatR;
using MyFactory.Application.DTOs.Authentication;

namespace MyFactory.Application.Features.Authentication.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<LoginDto>;
