namespace MyFactory.MauiClient.Models.Employees;

public record TimesheetListItemResponse(
    Guid EmployeeId,
    string EmployeeName,
    string DepartmentName,
    decimal TotalHours,
    int WorkDays);
