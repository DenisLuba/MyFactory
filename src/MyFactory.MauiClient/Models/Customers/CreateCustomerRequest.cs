namespace MyFactory.MauiClient.Models.Customers;

public record CreateCustomerRequest(
    string Name,
    string? Phone,
    string? Email,
    string? Address);
