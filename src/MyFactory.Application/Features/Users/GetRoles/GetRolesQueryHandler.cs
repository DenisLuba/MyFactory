using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetRoles;

public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyList<RoleDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRolesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Roles
            .AsNoTracking()
            .Select(role => new RoleDto(role.Id, role.Name))
            .ToListAsync(cancellationToken);
    }
}
