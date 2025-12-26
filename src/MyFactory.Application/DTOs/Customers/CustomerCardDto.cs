namespace MyFactory.Application.DTOs.Customers;

public sealed class CustomerCardDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public string? Address { get; init; }
    public IReadOnlyList<CustomerSalesOrderDto> Orders { get; init; } = new List<CustomerSalesOrderDto>();
}
