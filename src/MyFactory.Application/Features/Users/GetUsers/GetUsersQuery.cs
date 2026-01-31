using MediatR;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery(Guid? RoleId = null, string? RoleName = null, bool IncludeInactive = false, bool SortDesk = false) : IRequest<IReadOnlyList<UserListItemDto>>;
