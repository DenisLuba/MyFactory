namespace MyFactory.WebApi.Contracts.Advances;

public record CreateCashAdvanceReturnRequest(
    DateOnly ReturnDate,
    decimal Amount,
    string? Description);
