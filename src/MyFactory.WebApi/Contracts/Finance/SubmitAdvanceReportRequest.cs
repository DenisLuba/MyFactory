namespace MyFactory.WebApi.Contracts.Finance;

public record SubmitAdvanceReportRequest(
    // TODO: Добавить свойства для отчета по авансу
    decimal TotalSpent,
    string ReportDescription,
    List<AdvanceReportItem> Items
);

public record AdvanceReportItem(
    string ItemName,
    decimal Amount,
    string Category
);