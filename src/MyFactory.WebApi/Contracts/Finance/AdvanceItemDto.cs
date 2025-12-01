namespace MyFactory.WebApi.Contracts.Finance;

public record AdvanceItemDto(
    string AdvanceNumber,
    string Employee,
    decimal AdvanceAmount,
    string Date,
    AdvanceStatus Status
);