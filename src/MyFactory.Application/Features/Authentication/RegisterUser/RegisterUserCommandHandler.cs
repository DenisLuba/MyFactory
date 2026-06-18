using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Authentication;
using MyFactory.Domain.Entities.Security;

namespace MyFactory.Application.Features.Authentication.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IApplicationDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var usernameExists = await _db.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
        if (usernameExists)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        var roleExists = await _db.Roles.AnyAsync(r => r.Id == request.RoleId, cancellationToken);
        if (!roleExists)
        {
            throw new InvalidOperationException("Role not found.");
        }

        var passwordHash = await _passwordHasher.HashAsync(request.Password, cancellationToken);

        var user = new UserEntity(request.Username, passwordHash, request.RoleId);

        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return new RegisterUserDto(user.Id, user.Username, user.RoleId);
    }
}
