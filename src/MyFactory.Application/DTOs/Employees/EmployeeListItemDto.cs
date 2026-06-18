namespace MyFactory.Application.DTOs.Employees;

public sealed class EmployeeListItemDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = null!;
    public string DepartmentName { get; init; } = null!;
    public string PositionName { get; init; } = null!;
    public bool IsActive { get; init; }
}