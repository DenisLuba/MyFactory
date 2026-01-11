using MediatR;
using MyFactory.Application.DTOs.Authentication;

namespace MyFactory.Application.Features.Authentication.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<RefreshTokenDto>;