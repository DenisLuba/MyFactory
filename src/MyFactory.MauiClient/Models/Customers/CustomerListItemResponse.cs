namespace MyFactory.MauiClient.Models.Customers;

public record CustomerListItemResponse(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Address);
