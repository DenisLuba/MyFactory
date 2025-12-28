namespace MyFactory.Application.DTOs.Organization;

public sealed class TimesheetListItemDto
{
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = null!;
    public string DepartmentName { get; init; } = null!;
    public decimal TotalHours { get; init; }
    public int WorkDays { get; init; }
}