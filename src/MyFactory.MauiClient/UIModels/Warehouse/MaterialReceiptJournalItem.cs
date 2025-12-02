using System;

namespace MyFactory.MauiClient.UIModels.Warehouse;

public record MaterialReceiptJournalItem(
    Guid Id,
    string Document,
    string Date,
    string Supplier,
    decimal TotalAmount,
    string Status
);