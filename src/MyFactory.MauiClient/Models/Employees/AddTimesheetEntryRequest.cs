namespace MyFactory.MauiClient.Models.Employees;

public record AddTimesheetEntryRequest(
    DateOnly Date,
    decimal Hours,
    string? Comment);
