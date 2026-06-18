namespace MyFactory.Application.DTOs.MaterialTypes;

public sealed record MaterialTypeDetailsDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}
