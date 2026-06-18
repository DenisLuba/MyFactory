namespace MyFactory.WebApi.Contracts.Employees;

public record UpdateTimesheetEntryRequest(
    decimal Hours,
    string? Comment);
