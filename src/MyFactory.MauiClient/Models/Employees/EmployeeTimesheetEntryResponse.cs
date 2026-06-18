namespace MyFactory.MauiClient.Models.Employees;

public record EmployeeTimesheetEntryResponse(
    Guid EntryId,
    DateOnly Date,
    decimal Hours,
    string? Comment);
