using MyFactory.Domain.Entities.Organization;

namespace MyFactory.WebApi.Contracts.Departments;

public record DepartmentListItemResponse(
    Guid Id,
    string Code,
    string Name,
    DepartmentType Type,
    bool IsActive);
