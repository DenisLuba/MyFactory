namespace MyFactory.WebApi.Contracts.Customers;

public record CustomerCardResponse(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Address,
    IReadOnlyList<CustomerOrderItemResponse> Orders);
