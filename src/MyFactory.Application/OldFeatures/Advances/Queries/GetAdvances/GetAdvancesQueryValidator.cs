using System;
using FluentValidation;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvances;

public sealed class GetAdvancesQueryValidator : AbstractValidator<GetAdvancesQuery>
{
    public GetAdvancesQueryValidator()
    {
        RuleFor(query => query.EmployeeId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("Employee id must be empty or a valid value.");
    }
}
