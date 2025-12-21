using System;

namespace MyFactory.Application.OldDTOs.Identity;

public sealed record AuthResultDto(string AccessToken, string RefreshToken, DateTime ExpiresAt, UserDto User);
