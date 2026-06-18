using FluentValidation;

namespace MyFactory.Application.Features.Customers.GetCustomers;

public sealed class GetCustomersQueryValidator : AbstractValidator<GetCustomersQuery>
{
    private const int MaxTake = 100;
    private static readonly string[] AllowedSortFields =
    [
        "name",
        "created"
    ];

    public GetCustomersQueryValidator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100);

        RuleFor(x => x.SortBy)
            .Must(sort =>
                sort is null ||
                AllowedSortFields.Contains(sort.Trim().ToLowerInvariant()))
            .WithMessage("SortBy must be one of: name or created.");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, MaxTake);
    }
}
