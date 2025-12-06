using System;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    Task<TokenResult> GenerateTokensAsync(User user, CancellationToken cancellationToken = default);

    Task<TokenResult> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken = default);
}

public sealed record TokenResult(string AccessToken, string RefreshToken, DateTime ExpiresAt, Guid UserId);
