namespace MyFactory.WebApi.Contracts.Customers;

public record CustomerListItemResponse(
    Guid Id,
    string Name,
    IEnumerable<string?> Phones,
    IEnumerable<string?> Emails,
    IEnumerable<string?> Addresses,
    bool IsActive);
