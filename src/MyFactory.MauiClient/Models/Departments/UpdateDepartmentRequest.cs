namespace MyFactory.MauiClient.Models.Departments;

public record UpdateDepartmentRequest(
    string Name,
    string Code,
    DepartmentType Type,
    bool IsActive);
