namespace MyFactory.WebApi.Contracts.Finance;

public record SubmitAdvanceReportResponse(
    Guid AdvanceId,
    FinanceStatus Status
);
