using System;

namespace MyFactory.MauiClient.Models.WorkshopExpenses;

public record WorkshopExpenseCreateRequest(
    Guid WorkshopId,
    decimal AmountPerUnit,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
