using MediatR;
using MyFactory.Application.DTOs.Users;

namespace MyFactory.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery(Guid? RoleId = null, string? RoleName = null) : IRequest<IReadOnlyList<UserListItemDto>>;
