using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Commands.CreateShipment;

namespace MyFactory.Application.Features.FinishedGoods.Validators;

public sealed class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(command => command.ShipmentNumber).NotEmpty();
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.ShipmentDate)
            .Must(date => date != default)
            .WithMessage("Shipment date is required.");
        RuleFor(command => command.Items)
            .NotEmpty()
            .WithMessage("Shipment must contain at least one item.");

        RuleForEach(command => command.Items)
            .SetValidator(new CreateShipmentItemDtoValidator());
    }

    private sealed class CreateShipmentItemDtoValidator : AbstractValidator<CreateShipmentItemDto>
    {
        public CreateShipmentItemDtoValidator()
        {
            RuleFor(item => item.SpecificationId).NotEmpty();
            RuleFor(item => item.Quantity).GreaterThan(0m);
            RuleFor(item => item.UnitPrice).GreaterThanOrEqualTo(0m);
        }
    }
}
