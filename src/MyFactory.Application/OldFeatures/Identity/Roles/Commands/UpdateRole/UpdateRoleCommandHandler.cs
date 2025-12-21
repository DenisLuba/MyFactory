using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Identity;

namespace MyFactory.Application.OldFeatures.Identity.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(entity => entity.Id == request.RoleId, cancellationToken)
            ?? throw new InvalidOperationException("Role not found.");

        var normalizedName = request.Name.Trim();

        var duplicateExists = await _context.Roles
            .AsNoTracking()
            .AnyAsync(entity => entity.Id != request.RoleId && entity.Name == normalizedName, cancellationToken);

        if (duplicateExists)
        {
            throw new InvalidOperationException("Role with the same name already exists.");
        }

        role.Rename(normalizedName);
        role.UpdateDescription(request.Description);

        await _context.SaveChangesAsync(cancellationToken);
        return RoleDto.FromEntity(role);
    }
}
