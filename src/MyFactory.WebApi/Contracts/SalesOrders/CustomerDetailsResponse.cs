namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record CustomerDetailsResponse(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Address);
