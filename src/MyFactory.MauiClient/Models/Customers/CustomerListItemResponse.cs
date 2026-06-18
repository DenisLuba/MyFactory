using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.Customers;

public record CustomerListItemResponse(
    Guid Id,
    string Name,
    IEnumerable<string?> Phones,
    IEnumerable<string?> Emails,
    IEnumerable<string?> Addresses,
    bool IsActive) : ListItemResponse(Id, Name);
