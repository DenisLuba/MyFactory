using System;

namespace MyFactory.WebApi.Contracts.WorkshopExpenses;

public record WorkshopExpenseGetResponse(
    Guid Id,
    Guid WorkshopId,
    decimal AmountPerUnit,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo
);
