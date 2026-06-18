using FluentValidation;

namespace MyFactory.Application.Features.Shipments.DeleteShipment;

public sealed class DeleteShipmentCommandValidator : AbstractValidator<DeleteShipmentCommand>
{
    public DeleteShipmentCommandValidator()
    {
        RuleFor(x => x.ShipmentId).NotEmpty();
    }
}
