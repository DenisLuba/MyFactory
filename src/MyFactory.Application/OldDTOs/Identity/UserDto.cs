using System;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.OldDTOs.Identity;

public sealed record UserDto(Guid Id, string Username, string Email, Guid RoleId, bool IsActive, DateTime CreatedAt)
{
    public static UserDto FromEntity(User user)
        => new(user.Id, user.Username, user.Email, user.RoleId, user.IsActive, user.CreatedAt);
}
