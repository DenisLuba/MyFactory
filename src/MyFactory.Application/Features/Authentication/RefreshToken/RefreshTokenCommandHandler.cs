using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Authentication;

namespace MyFactory.Application.Features.Authentication.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IApplicationDbContext db, IJwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RefreshTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = await _jwtTokenService.RefreshTokensAsync(request.RefreshToken, cancellationToken);

        var user = await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == tokenResult.UserId, cancellationToken)
            ?? throw new InvalidOperationException("User not found.");

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User is inactive.");
        }

        return new RefreshTokenDto(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.ExpiresAt);
    }
}
