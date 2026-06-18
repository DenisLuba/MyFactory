using FluentValidation;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrders;

public sealed class GetSalesOrdersQueryValidator 
    : AbstractValidator<GetSalesOrdersQuery>
{
    private const int MaxTake = 100;
    private static readonly string[] AllowedSortFields =
    {
        "date",
        "number",
        "customername"
    };

    public GetSalesOrdersQueryValidator()
    {
        RuleFor(x => x.SearchName)
            .MaximumLength(100);

        RuleFor(x => x.SortBy)
            .Must(sort =>
                sort is null ||
                AllowedSortFields.Contains(sort.Trim().ToLowerInvariant()))
            .WithMessage("SortBy must be one of: date, number or customername");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(x => x.Take)
            .InclusiveBetween(1, MaxTake);
    }
}
