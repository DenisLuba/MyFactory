namespace MyFactory.WebApi.Contracts.Auth;

public record RegisterRequest(string UserName, Guid RoleId, string Password);

