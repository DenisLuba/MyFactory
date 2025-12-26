namespace MyFactory.Application.DTOs.Customers;

public sealed class CustomerListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
}
