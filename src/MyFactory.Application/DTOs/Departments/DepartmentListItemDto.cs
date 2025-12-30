using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.DTOs.Departments;

public class DepartmentListItemDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = null!;
    public string Name { get; init; } = null!;
    public DepartmentType Type { get; init; }
    public bool IsActive { get; init; }
}

