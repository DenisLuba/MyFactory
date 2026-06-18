using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Users.DeactivateUser;

public sealed class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IApplicationDbContext _context;

    public DeactivateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken) ?? throw new InvalidOperationException($"User with id {request.UserId} not found.");
        
        if (user.IsActive)
        {
            user.Deactivate();
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
