namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeListItemResponse(
    Guid Id,
    string FullName,
    string DepartmentName,
    string PositionName,
    bool IsActive);
