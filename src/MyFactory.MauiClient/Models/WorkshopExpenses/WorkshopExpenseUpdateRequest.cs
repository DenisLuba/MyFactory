using System;

namespace MyFactory.MauiClient.Models.WorkshopExpenses;

public record WorkshopExpenseUpdateRequest(
    Guid WorkshopId,
    decimal AmountPerUnit,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
