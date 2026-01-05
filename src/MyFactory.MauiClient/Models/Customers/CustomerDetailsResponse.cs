namespace MyFactory.MauiClient.Models.Customers;

public record CustomerDetailsResponse(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Address);
