namespace MyFactory.WebApi.Contracts.Employees;

public record AddTimesheetEntryRequest(
    DateOnly Date,
    decimal Hours,
    string? Comment);
