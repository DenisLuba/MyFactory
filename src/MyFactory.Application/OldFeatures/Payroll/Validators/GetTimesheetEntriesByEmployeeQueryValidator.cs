using FluentValidation;
using MyFactory.Application.Features.Payroll.Queries.GetTimesheetEntriesByEmployee;

namespace MyFactory.Application.OldFeatures.Payroll.Validators;

public sealed class GetTimesheetEntriesByEmployeeQueryValidator : AbstractValidator<GetTimesheetEntriesByEmployeeQuery>
{
    public GetTimesheetEntriesByEmployeeQueryValidator()
    {
        RuleFor(query => query.EmployeeId).NotEmpty();

        RuleFor(query => query)
            .Must(query => !query.FromDate.HasValue || !query.ToDate.HasValue || query.ToDate.Value >= query.FromDate.Value)
            .WithMessage("To date cannot be before from date.");
    }
}
