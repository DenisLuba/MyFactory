using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Queries.GetShipmentById;

namespace MyFactory.Application.Features.FinishedGoods.Validators;

public sealed class GetShipmentByIdQueryValidator : AbstractValidator<GetShipmentByIdQuery>
{
    public GetShipmentByIdQueryValidator()
    {
        RuleFor(query => query.ShipmentId).NotEmpty();
    }
}
