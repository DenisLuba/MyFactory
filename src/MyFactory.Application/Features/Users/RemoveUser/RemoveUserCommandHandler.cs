using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFactory.Application.Features.Users.RemoveUser;

public sealed class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand>
{
    private readonly IApplicationDbContext _context;

    public RemoveUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RemoveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException($"User with id {request.UserId} not found.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
