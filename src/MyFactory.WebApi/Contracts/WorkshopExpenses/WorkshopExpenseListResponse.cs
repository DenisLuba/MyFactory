using System;

namespace MyFactory.WebApi.Contracts.WorkshopExpenses;

public record WorkshopExpenseListResponse(
    Guid Id,
    Guid WorkshopId,
    string Workshop,
    decimal AmountPerUnit,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
