namespace MyFactory.Application.DTOs.Employees;

public sealed class ContactDto
{
    public Guid Id { get; init; }
    public string Type { get; init; } = null!;
    public string Value { get; init; } = null!;
}