namespace MyFactory.WebApi.Contracts.Advances;

public record AddCashAdvanceAmountRequest(
    DateOnly IssueDate,
    decimal Amount);
