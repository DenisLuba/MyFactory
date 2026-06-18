using MyFactory.Domain.Entities.Organization;

namespace MyFactory.WebApi.Contracts.Departments;

public record DepartmentDetailsResponse(
    Guid Id,
    string Code,
    string Name,
    DepartmentType Type,
    bool IsActive);
