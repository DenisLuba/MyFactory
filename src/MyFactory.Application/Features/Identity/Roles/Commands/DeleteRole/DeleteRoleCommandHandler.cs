using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, RoleDto>
{
    private readonly IApplicationDbContext _context;

    public DeleteRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoleDto> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(entity => entity.Id == request.RoleId, cancellationToken)
            ?? throw new InvalidOperationException("Role not found.");

        var hasUsers = await _context.Users
            .AsNoTracking()
            .AnyAsync(user => user.RoleId == request.RoleId, cancellationToken);

        if (hasUsers)
        {
            throw new InvalidOperationException("Role cannot be deleted while assigned to users.");
        }

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(cancellationToken);

        return RoleDto.FromEntity(role);
    }
}
