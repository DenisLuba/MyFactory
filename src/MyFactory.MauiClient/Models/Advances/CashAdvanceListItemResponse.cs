namespace MyFactory.MauiClient.Models.Advances;

public record CashAdvanceListItemResponse(
    Guid Id,
    DateOnly IssueDate,
    string EmployeeName,
    decimal IssuedAmount,
    decimal SpentAmount,
    decimal ReturnedAmount,
    decimal Balance,
    bool IsClosed);

