namespace MyFactory.WebApi.Contracts.Customers;

public record CustomerDetailsResponse(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Address);