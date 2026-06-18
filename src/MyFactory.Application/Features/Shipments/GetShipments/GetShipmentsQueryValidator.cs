using FluentValidation;

namespace MyFactory.Application.Features.Shipments.GetShipments;

public sealed class GetShipmentsQueryValidator : AbstractValidator<GetShipmentsQuery>
{
    public GetShipmentsQueryValidator()
    {
        RuleFor(x => x.FromDate)
            .Must((query, from) => !from.HasValue || !query.ToDate.HasValue || from <= query.ToDate)
            .WithMessage("FromDate must be earlier or equal to ToDate");
    }
}
