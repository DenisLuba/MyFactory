namespace MyFactory.Application.DTOs.Positions;

public sealed class PositionListItemDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;
    public string? Code { get; init; }

    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = null!;

    public bool IsActive { get; init; }
}
