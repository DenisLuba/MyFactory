namespace MyFactory.MauiClient.Models.Advances;

public record AddCashAdvanceAmountRequest(
    DateOnly IssueDate,
    decimal Amount);
