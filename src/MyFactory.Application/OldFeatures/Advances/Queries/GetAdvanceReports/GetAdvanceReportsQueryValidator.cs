using FluentValidation;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvanceReports;

public sealed class GetAdvanceReportsQueryValidator : AbstractValidator<GetAdvanceReportsQuery>
{
    public GetAdvanceReportsQueryValidator()
    {
        RuleFor(query => query.AdvanceId).NotEmpty();
    }
}
