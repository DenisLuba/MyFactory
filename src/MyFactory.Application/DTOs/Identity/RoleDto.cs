using System;
using MyFactory.Domain.Entities.Identity;

namespace MyFactory.Application.DTOs.Identity;

public sealed record RoleDto(Guid Id, string Name, string? Description, DateTime CreatedAt)
{
    public static RoleDto FromEntity(Role role)
        => new(role.Id, role.Name, role.Description, role.CreatedAt);
}
