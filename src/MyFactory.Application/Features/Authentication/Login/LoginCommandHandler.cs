using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Authentication;

namespace MyFactory.Application.Features.Authentication.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IApplicationDbContext db,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken)
            ?? throw new InvalidOperationException("Invalid credentials.");

        if (!user.IsActive)
        {
            throw new InvalidOperationException("User is inactive.");
        }

        var passwordOk = await _passwordHasher.VerifyAsync(request.Password, user.PasswordHash, cancellationToken);
        if (!passwordOk)
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        var tokenResult = await _jwtTokenService.GenerateTokensAsync(user, cancellationToken);

        return new LoginDto(tokenResult.AccessToken, tokenResult.RefreshToken, tokenResult.ExpiresAt);
    }
}
