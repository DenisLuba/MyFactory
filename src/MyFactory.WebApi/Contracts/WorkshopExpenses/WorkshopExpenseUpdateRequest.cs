using System;
using System.ComponentModel.DataAnnotations;

namespace MyFactory.WebApi.Contracts.WorkshopExpenses;

public record WorkshopExpenseUpdateRequest(
    Guid WorkshopId,
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")] decimal AmountPerUnit,
    [DataType(DataType.Date)] DateTime EffectiveFrom,
    [DataType(DataType.Date)] DateTime? EffectiveTo
) : WorkshopExpenseCreateRequest(WorkshopId, AmountPerUnit, EffectiveFrom, EffectiveTo);
