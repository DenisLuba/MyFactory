namespace MyFactory.Application.DTOs.Units;

public sealed class UnitDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = default!;
    public string Name { get; init; } = default!;
}
