namespace MyFactory.Application.DTOs.Customers;

public sealed class CustomerListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public IEnumerable<string?> Phones { get; init; } = [];
    public IEnumerable<string?> Emails { get; init; } = [];
    public IEnumerable<string?> Addresses { get; init; } = [];
    public bool IsActive { get; init; }
}
