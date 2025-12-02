using System;

namespace MyFactory.MauiClient.Models.WorkshopExpenses;

public record WorkshopExpenseListResponse(
    Guid Id,
    Guid WorkshopId,
    string Workshop,
    decimal AmountPerUnit,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
