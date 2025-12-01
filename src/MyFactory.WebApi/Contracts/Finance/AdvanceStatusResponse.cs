namespace MyFactory.WebApi.Contracts.Finance;

public record AdvanceStatusResponse(
    Guid id,
    AdvanceStatus Status
);