namespace MyFactory.WebApi.Contracts.SalesOrders;

public sealed record SalesOrderCustomerDetailsResponse(
    Guid Id,
    string Name,
    string? Phone,
    string? Email,
    string? Address);
