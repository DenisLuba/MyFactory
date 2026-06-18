using MyFactory.Domain.Entities.Organization;

namespace MyFactory.WebApi.Contracts.Departments;

public record UpdateDepartmentRequest(
    string Name,
    string Code,
    DepartmentType Type,
    bool IsActive);
