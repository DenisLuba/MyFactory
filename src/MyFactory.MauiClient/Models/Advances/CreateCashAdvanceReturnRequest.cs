namespace MyFactory.MauiClient.Models.Advances;

public record CreateCashAdvanceReturnRequest(
    DateOnly ReturnDate,
    decimal Amount,
    string? Description);
