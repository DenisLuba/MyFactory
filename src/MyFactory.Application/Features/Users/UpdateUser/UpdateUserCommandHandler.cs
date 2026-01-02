using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException($"User with id {request.UserId} not found.");
        }

        var roleExists = await _context.Roles
            .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

        if (!roleExists)
        {
            throw new InvalidOperationException($"Role with id {request.RoleId} not found.");
        }

        if (user.RoleId != request.RoleId)
        {
            user.ChangeRoleId(request.RoleId);
        }

        if (request.IsActive && !user.IsActive)
        {
            user.Activate();
        }
        else if (!request.IsActive && user.IsActive)
        {
            user.Deactivate();
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
