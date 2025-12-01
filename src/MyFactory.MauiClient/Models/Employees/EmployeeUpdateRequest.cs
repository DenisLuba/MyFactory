namespace MyFactory.MauiClient.Models.Employees;

public record EmployeeUpdateRequest(
    string FullName,
    string Position,
    int Grade,
    bool IsActive
);
