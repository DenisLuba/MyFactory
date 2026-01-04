namespace MyFactory.WebApi.Contracts.Customers;

public record CreateCustomerRequest(
    string Name,
    string? Phone,
    string? Email,
    string? Address);
