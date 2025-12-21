using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Queries.GetShipmentById;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Validators;

public sealed class GetShipmentByIdQueryValidator : AbstractValidator<GetShipmentByIdQuery>
{
    public GetShipmentByIdQueryValidator()
    {
        RuleFor(query => query.ShipmentId).NotEmpty();
    }
}
