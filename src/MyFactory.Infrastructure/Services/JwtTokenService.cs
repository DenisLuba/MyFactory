using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Security;
using MyFactory.Infrastructure.Common;

namespace MyFactory.Infrastructure.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IApplicationDbContext _db;
    private readonly JwtOptions _options;

    public JwtTokenService(IApplicationDbContext db, IOptions<JwtOptions> options)
    {
        _db = db;
        _options = options.Value;
    }

    public async Task<TokenResult> GenerateTokensAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        var refreshToken = CreateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);
        var accessToken = CreateAccessToken(user, expiresAt);

        var tokenEntity = new TokenEntity(user.Id, refreshToken, expiresAt);
        await _db.Tokens.AddAsync(tokenEntity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return new TokenResult(accessToken, refreshToken, expiresAt, user.Id);
    }

    public async Task<TokenResult> RefreshTokensAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var existing = await _db.Tokens
            .Include(t => t.User)
            .SingleOrDefaultAsync(t => t.RefreshToken == refreshToken, cancellationToken)
            ?? throw new InvalidOperationException("Invalid refresh token.");

        if (existing.IsRevoked || existing.IsExpired)
        {
            throw new InvalidOperationException("Invalid refresh token.");
        }

        if (existing.User is null || !existing.User.IsActive)
        {
            throw new InvalidOperationException("User is inactive or missing.");
        }

        existing.Revoke(DateTime.UtcNow);

        var newRefreshToken = CreateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);
        var accessToken = CreateAccessToken(existing.User, expiresAt);

        var newToken = new TokenEntity(existing.UserId, newRefreshToken, expiresAt);
        await _db.Tokens.AddAsync(newToken, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        return new TokenResult(accessToken, newRefreshToken, expiresAt, existing.UserId);
    }

    private string CreateAccessToken(UserEntity user, DateTime expiresAt)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private static string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
