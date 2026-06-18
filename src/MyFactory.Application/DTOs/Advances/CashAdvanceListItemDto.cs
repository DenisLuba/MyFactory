namespace MyFactory.Application.DTOs.Advances;

public sealed record CashAdvanceListItemDto(
    Guid Id,
    DateOnly IssueDate,
    string EmployeeName,
    decimal IssuedAmount,
    decimal SpentAmount,
    decimal ReturnedAmount,
    decimal Balance,
    bool IsClosed
);
