using FluentValidation;

namespace MyFactory.Application.Features.Shipments.GetShipmentDetails;

public sealed class GetShipmentDetailsQueryValidator : AbstractValidator<GetShipmentDetailsQuery>
{
    public GetShipmentDetailsQueryValidator()
    {
        RuleFor(x => x.ShipmentId).NotEmpty();
    }
}
