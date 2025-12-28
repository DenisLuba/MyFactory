namespace MyFactory.Application.DTOs.Organization;

public sealed class PositionDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string DepartmentName { get; init; } = null!;
}