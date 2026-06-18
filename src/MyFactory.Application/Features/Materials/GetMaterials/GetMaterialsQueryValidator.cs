using FluentValidation;

namespace MyFactory.Application.Features.Materials.GetMaterials;

public sealed class GetMaterialsQueryValidator : AbstractValidator<GetMaterialsQuery>
{
    private const int MaxTake = 100;
    private static readonly string[] AllowedSortFields =
    {
        "type",
        "name",
        "qty"
    };

    public GetMaterialsQueryValidator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100);

        RuleFor(x => x.SearchType)
            .MaximumLength(100);

        RuleFor(x => x.SortBy)
            .Must(sort =>
                sort is null ||
                AllowedSortFields.Contains(sort.Trim().ToLowerInvariant()))
            .WithMessage("SortBy must be one of: name, type, qty");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, MaxTake);
    }
}
