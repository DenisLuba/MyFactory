namespace MyFactory.MauiClient.Models.Employees;

public record UpdateTimesheetEntryRequest(
    decimal Hours,
    string? Comment);
