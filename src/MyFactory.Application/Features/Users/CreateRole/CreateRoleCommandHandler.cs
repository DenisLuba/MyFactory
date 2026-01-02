using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Security;

namespace MyFactory.Application.Features.Users.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Roles
            .AnyAsync(role => role.Name == request.Name, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException($"Role '{request.Name}' already exists.");
        }

        var role = new RoleEntity(request.Name);

        _context.Roles.Add(role);
        await _context.SaveChangesAsync(cancellationToken);

        return role.Id;
    }
}
