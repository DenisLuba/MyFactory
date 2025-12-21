using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Identity;
using MyFactory.Application.Interfaces.Auth;

namespace MyFactory.Application.OldFeatures.Identity.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var identifier = request.UsernameOrEmail.Trim();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == identifier || u.Email == identifier, cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        var passwordMatches = await _passwordHasher.VerifyAsync(request.Password, user.PasswordHash, cancellationToken);
        if (!passwordMatches)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User is inactive.");
        }

        var tokenResult = await _jwtTokenService.GenerateTokensAsync(user, cancellationToken);

        return new AuthResultDto(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.ExpiresAt, UserDto.FromEntity(user));
    }
}
