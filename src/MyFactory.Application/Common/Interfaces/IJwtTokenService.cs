using MyFactory.Domain.Entities.Security;

namespace MyFactory.Application.Common.Interfaces;

public interface IJwtTokenService
{
    Task<TokenResult> GenerateTokensAsync(UserEntity user, CancellationToken cancellationToken = default);

    Task<TokenResult> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken = default);
}

public sealed record TokenResult(string AccessToken, string RefreshToken, DateTime ExpiresAt, Guid UserId);
