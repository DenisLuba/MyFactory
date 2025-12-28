namespace MyFactory.Application.DTOs.Organization;

public sealed class ContactDto
{
    public Guid Id { get; init; }
    public string Type { get; init; } = null!;
    public string Value { get; init; } = null!;
}