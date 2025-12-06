using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Identity;
using MyFactory.Application.Interfaces.Auth;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.Features.Identity.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        var email = request.Email.Trim();

        if (await _context.Users.AnyAsync(user => user.Username == username, cancellationToken))
        {
            throw new InvalidOperationException("Username already exists.");
        }

        if (await _context.Users.AnyAsync(user => user.Email == email, cancellationToken))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var roleExists = await _context.Roles.AnyAsync(role => role.Id == request.RoleId, cancellationToken);
        if (!roleExists)
        {
            throw new InvalidOperationException("Role does not exist.");
        }

        var passwordHash = await _passwordHasher.HashAsync(request.Password, cancellationToken);
        var user = new User(username, email, passwordHash, request.RoleId);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return UserDto.FromEntity(user);
    }
}
