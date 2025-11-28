namespace MyFactory.MauiClient.Models.Payroll;

public record PayrollCalculateResponse(
    PayrollCalculatingStatus Status,
    DateTime From,
    DateTime To);
