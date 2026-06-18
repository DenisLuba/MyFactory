using FluentValidation;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrders;

public sealed class GetProductionOrdersQueryValidator : AbstractValidator<GetProductionOrdersQuery>
{
    private const int MaxTake = 100;
    private static readonly string[] AllowedSortFields =
    {
        "date",
        "number",
        "customername",
        "salesordernumber",
        "productname"
    };

    public GetProductionOrdersQueryValidator()
    {
        RuleFor(x => x.SearchProductionOrderNumber)
            .MaximumLength(12);

        RuleFor(x => x.SearchSaleOrderNumber)
            .MaximumLength(12);

        RuleFor(x => x.SearchCustomerName)
            .MaximumLength(100);

        RuleFor(x => x.SearchProductName)
            .MaximumLength(100);

        RuleFor(x => x.SortBy)
            .Must(sort =>
                sort is null ||
                AllowedSortFields.Contains(sort.Trim().ToLowerInvariant()))
            .WithMessage("SortBy must be one of: date, number, salesordernumber, productname, or customername");

        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, MaxTake);

        RuleFor(x => x)
            .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.FromDate <= x.ToDate)
            .WithMessage("FromDate must be less than or equal to ToDate");
    }
}
