namespace MyFactory.WebApi.Contracts.Advances;

public record CashAdvanceListItemResponse(
    Guid Id,
    DateOnly IssueDate,
    string EmployeeName,
    decimal IssuedAmount,
    decimal SpentAmount,
    decimal ReturnedAmount,
    decimal Balance,
    bool IsClosed);
