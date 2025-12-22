using FluentValidation;

namespace MyFactory.Application.Features.Products.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    private static readonly string[] AllowedSortFields =
    {
        "name",
        "cost"
    };

    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100);

        RuleFor(x => x.SortBy)
            .Must(sort =>
                sort is null ||
                AllowedSortFields.Contains(sort.ToLowerInvariant()))
            .WithMessage("SortBy must be one of: name, cost");
    }
}
