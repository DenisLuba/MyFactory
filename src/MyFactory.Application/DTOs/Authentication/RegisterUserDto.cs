namespace MyFactory.Application.DTOs.Authentication;

public record RegisterUserDto(
    Guid UserId,
    string Username,
    Guid RoleId);
