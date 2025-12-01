namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeUpdateRequest(
    string FullName,
    string Position,
    int Grade,
    bool IsActive
);
