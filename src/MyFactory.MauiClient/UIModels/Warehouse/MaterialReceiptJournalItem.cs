namespace MyFactory.MauiClient.UIModels.Warehouse;

// Журнал поступления материалов
public record MaterialReceiptJournalItem(
    string Document,
    string Date,
    string Supplier,
    decimal TotalAmount,
    string Status
);