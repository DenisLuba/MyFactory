namespace MyFactory.WebApi.Contracts.Employees;

public record EmployeeTimesheetEntryResponse(
    Guid EntryId,
    DateOnly Date,
    decimal Hours,
    string? Comment);
