namespace MyFactory.MauiClient.Models.Finance;

public record SubmitAdvanceReportResponse(
    Guid AdvanceId,
    FinanceStatus Status);

