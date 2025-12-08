using System;

namespace MyFactory.MauiClient.Models.Finance;

public record AdvanceReportItem(
    string ItemName,
    DateTime ExpenseDate,
    decimal Amount,
    string Comment,
    AdvanceReportCategories Category,
    Guid ReceiptFileId,
    string? ReceiptUri = null);

