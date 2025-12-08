using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Identity;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.Features.Identity.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IApplicationDbContext _context;

    public CreateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim();

        var exists = await _context.Roles
            .AsNoTracking()
            .AnyAsync(role => role.Name == normalizedName, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Role with the same name already exists.");
        }

        var role = new Role(normalizedName, request.Description);
        await _context.Roles.AddAsync(role, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return RoleDto.FromEntity(role);
    }
}
