namespace MyFactory.MauiClient.Models.Finance;

public record SubmitAdvanceReportRequest(
    decimal TotalSpent,
    string ReportDescription,
    List<AdvanceReportItem> Items);
