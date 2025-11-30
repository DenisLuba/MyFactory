namespace MyFactory.WebApi.Contracts.Finance;

public enum AdvanceStatus
{
    Issued,
    Reported,
    Cancelled,
    Pending
}

public record AdvanceItemDto(
    string AdvanceNumber,
    string Employee,
    decimal AdvanceAmount,
    string Date,
    AdvanceStatus Status
);