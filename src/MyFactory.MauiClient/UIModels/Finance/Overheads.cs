using System;

namespace MyFactory.MauiClient.UIModels.Finance;

// Накладной расход для отображения в UI
public record OverheadItem(
    Guid Id,
    DateTime Date,
    string Article,
    decimal Amount,
    string Comment,
    OverheadStatus Status
);

