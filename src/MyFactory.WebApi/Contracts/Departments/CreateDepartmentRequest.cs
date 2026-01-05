using MyFactory.Domain.Entities.Organization;

namespace MyFactory.WebApi.Contracts.Departments;

public record CreateDepartmentRequest(
    string Name,
    string Code,
    DepartmentType Type);
