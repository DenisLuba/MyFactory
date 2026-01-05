namespace MyFactory.WebApi.Contracts.Employees;

public record TimesheetListItemResponse(
    Guid EmployeeId,
    string EmployeeName,
    string DepartmentName,
    decimal TotalHours,
    int WorkDays);
