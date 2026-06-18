using MyFactory.MauiClient.Models.Common;

namespace MyFactory.MauiClient.Models.Employees;

public record EmployeeListItemResponse(
    Guid Id,
    string FullName,
    string DepartmentName,
    string PositionName,
    bool IsActive) : ListItemResponse(Id, FullName);
