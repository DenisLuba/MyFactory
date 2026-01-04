namespace MyFactory.WebApi.Contracts.Customers;

public record UpdateCustomerRequest(
    string Name,
    string? Phone,
    string? Email,
    string? Address);
