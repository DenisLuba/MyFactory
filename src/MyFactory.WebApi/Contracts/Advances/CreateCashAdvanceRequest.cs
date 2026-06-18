namespace MyFactory.WebApi.Contracts.Advances;

public record CreateCashAdvanceRequest(
    Guid EmployeeId,
    DateOnly IssueDate,
    decimal Amount,
    string? Description);
