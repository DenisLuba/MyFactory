using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetUserDetails;

public sealed class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserDetailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDetailsDto> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var query = from user in _context.Users.AsNoTracking()
                    join role in _context.Roles.AsNoTracking() on user.RoleId equals role.Id
                    where user.Id == request.UserId
                    select new UserDetailsDto(user.Id, user.Username, role.Id, role.Name, user.IsActive, user.CreatedAt);

        var result = await query.FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException($"User with id {request.UserId} not found.");
        }

        return result;
    }
}
