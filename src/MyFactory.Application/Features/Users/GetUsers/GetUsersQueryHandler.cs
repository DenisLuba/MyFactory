using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserListItemDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = from user in _context.Users.AsNoTracking()
                    join role in _context.Roles.AsNoTracking() on user.RoleId equals role.Id
                    select new { user, role };

        if (request.RoleId.HasValue)
        {
            query = query.Where(x => x.role.Id == request.RoleId.Value);
        }
        else if (!string.IsNullOrWhiteSpace(request.RoleName))
        {
            query = query.Where(x => x.role.Name == request.RoleName);
        }

        if (!request.IncludeInactive)
        {
            query = query.Where(x => x.user.IsActive);
        }

        if (request.SortDesk)
        {
            query = query.OrderByDescending(x => x.user.Username);
        }
        else
        {
            query = query.OrderBy(x => x.user.Username);
        }

        return await query
            .Select(x => new UserListItemDto(
                x.user.Id,
                x.user.Username,
                x.role.Name,
                x.user.IsActive,
                x.user.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
