using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Security;

namespace MyFactory.Application.Features.Users.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

        if (!roleExists)
        {
            throw new InvalidOperationException($"Role with id {request.RoleId} not found.");
        }

        var usernameTaken = await _context.Users
            .AnyAsync(u => u.Username == request.Username, cancellationToken);

        if (usernameTaken)
        {
            throw new InvalidOperationException($"User '{request.Username}' already exists.");
        }

        var user = new UserEntity(request.Username, request.Password, request.RoleId);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
