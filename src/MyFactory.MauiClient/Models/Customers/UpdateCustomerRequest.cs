namespace MyFactory.MauiClient.Models.Customers;

public record UpdateCustomerRequest(
    string Name,
    string? Phone,
    string? Email,
    string? Address);
