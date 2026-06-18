using FluentValidation;

namespace MyFactory.Application.Features.Units.GetUnits;

public sealed class GetUnitsQueryValidator : AbstractValidator<GetUnitsQuery>
{
    public GetUnitsQueryValidator()
    {
        // no filters yet
    }
}
