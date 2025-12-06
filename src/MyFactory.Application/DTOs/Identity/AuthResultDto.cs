using System;

namespace MyFactory.Application.DTOs.Identity;

public sealed record AuthResultDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);
