using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Identity;
using MyFactory.Application.Interfaces.Auth;

namespace MyFactory.Application.OldFeatures.Identity.Commands.RefreshToken;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = await _jwtTokenService.RefreshTokensAsync(request.RefreshToken, cancellationToken);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == tokenResult.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User linked to token was not found.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User is inactive.");
        }

        return new AuthResultDto(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.ExpiresAt, UserDto.FromEntity(user));
    }
}
