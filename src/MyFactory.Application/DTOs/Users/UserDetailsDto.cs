using System;

namespace MyFactory.Application.DTOs.Users;

public sealed record UserDetailsDto(
    Guid Id,
    string Username,
    Guid RoleId,
    string RoleName,
    bool IsActive,
    DateTime CreatedAt);
