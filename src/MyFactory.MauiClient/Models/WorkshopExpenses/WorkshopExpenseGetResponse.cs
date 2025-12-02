using System;

namespace MyFactory.MauiClient.Models.WorkshopExpenses;

public record WorkshopExpenseGetResponse(
    Guid Id,
    Guid WorkshopId,
    decimal AmountPerUnit,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
