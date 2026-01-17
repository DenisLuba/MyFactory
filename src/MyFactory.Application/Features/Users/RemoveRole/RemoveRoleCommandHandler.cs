using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Users.DeactivateRole;

public sealed class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public RemoveRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);

        if (role is null)
        {
            throw new InvalidOperationException($"Role with id {request.RoleId} not found.");
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
