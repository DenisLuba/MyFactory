namespace MyFactory.MauiClient.Models.Departments;

public record DepartmentDetailsResponse(
    Guid Id,
    string Code,
    string Name,
    DepartmentType Type,
    bool IsActive);
