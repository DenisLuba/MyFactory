namespace MyFactory.MauiClient.Models.Advances;

public record CreateCashAdvanceRequest(
    Guid EmployeeId,
    DateOnly IssueDate,
    decimal Amount,
    string? Description);
