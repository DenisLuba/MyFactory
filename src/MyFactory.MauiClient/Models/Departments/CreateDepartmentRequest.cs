namespace MyFactory.MauiClient.Models.Departments;

public record CreateDepartmentRequest(
    string Name,
    string Code,
    DepartmentType Type);
