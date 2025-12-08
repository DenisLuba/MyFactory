using System;
using System.Collections.Generic;

namespace MyFactory.WebApi.Contracts.Finance;

public record SubmitAdvanceReportRequest(
    decimal TotalSpent,
    string ReportDescription,
    List<AdvanceReportItem> Items
);

public record AdvanceReportItem(
    string ItemName,
    DateTime ExpenseDate,
    decimal Amount,
    string Comment,
    AdvanceReportCategories Category,
    Guid ReceiptFileId,
    string? ReceiptUri = null
);

public enum AdvanceReportCategories
{
    Finance,
    Inventory,
    Payroll,
    Suppliers
}
