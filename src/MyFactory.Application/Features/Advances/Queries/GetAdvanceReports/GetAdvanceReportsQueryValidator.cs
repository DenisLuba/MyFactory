using FluentValidation;

namespace MyFactory.Application.Features.Advances.Queries.GetAdvanceReports;

public sealed class GetAdvanceReportsQueryValidator : AbstractValidator<GetAdvanceReportsQuery>
{
    public GetAdvanceReportsQueryValidator()
    {
        RuleFor(query => query.AdvanceId).NotEmpty();
    }
}
