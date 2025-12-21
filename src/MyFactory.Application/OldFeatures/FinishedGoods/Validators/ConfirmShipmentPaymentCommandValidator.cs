using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Commands.ConfirmShipmentPayment;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Validators;

public sealed class ConfirmShipmentPaymentCommandValidator : AbstractValidator<ConfirmShipmentPaymentCommand>
{
    public ConfirmShipmentPaymentCommandValidator()
    {
        RuleFor(command => command.ShipmentId).NotEmpty();
    }
}
