using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyFactory.WebApi.Contracts.WorkshopExpenses;

public record WorkshopExpenseCreateRequest(
    Guid WorkshopId,
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")] decimal AmountPerUnit,
    [DataType(DataType.Date)] DateTime EffectiveFrom,
    [DataType(DataType.Date)] DateTime? EffectiveTo
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (WorkshopId == Guid.Empty)
        {
            yield return new ValidationResult("WorkshopId is required.", new[] { nameof(WorkshopId) });
        }

        if (AmountPerUnit <= 0)
        {
            yield return new ValidationResult("AmountPerUnit must be greater than zero.", new[] { nameof(AmountPerUnit) });
        }

        if (EffectiveFrom == default)
        {
            yield return new ValidationResult("EffectiveFrom must be specified.", new[] { nameof(EffectiveFrom) });
        }

        if (EffectiveTo.HasValue && EffectiveTo.Value < EffectiveFrom)
        {
            yield return new ValidationResult("EffectiveTo must be greater than or equal to EffectiveFrom.", new[] { nameof(EffectiveTo) });
        }
    }
}
