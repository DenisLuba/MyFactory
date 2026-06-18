namespace MyFactory.Application.DTOs.Employees;

public sealed class EmployeeTimesheetEntryDto
{
    public Guid EntryId { get; init; }
    public DateOnly Date { get; init; }
    public decimal Hours { get; init; }
    public string? Comment { get; init; }
}