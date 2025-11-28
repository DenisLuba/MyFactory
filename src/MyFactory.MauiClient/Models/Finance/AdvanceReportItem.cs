namespace MyFactory.MauiClient.Models.Finance;

public record AdvanceReportItem(
    string ItemName,
    decimal Amount,
    AdvanceReportCategories Category);

