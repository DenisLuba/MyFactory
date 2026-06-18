namespace MyFactory.Application.DTOs.Departments;

public sealed class DepartmentDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
}