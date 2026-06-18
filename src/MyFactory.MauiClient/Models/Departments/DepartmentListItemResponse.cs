namespace MyFactory.MauiClient.Models.Departments;

public record DepartmentListItemResponse(
    Guid Id,
    string Code,
    string Name,
    DepartmentType Type,
    bool IsActive);
