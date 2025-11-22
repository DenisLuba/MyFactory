namespace MyFactory.WebApi.Contracts.Auth;

public record RegisterRequest(string UserName, string Email, string Password);
