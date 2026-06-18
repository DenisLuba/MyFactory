using FluentValidation;
using MyFactory.Application.Features.Shipments.CreateShipment;

namespace MyFactory.Application.Features.Shipments.UpdateShipment;

public sealed class UpdateShipmentCommandValidator : AbstractValidator<UpdateShipmentCommand>
{
    public UpdateShipmentCommandValidator()
    {
        RuleFor(x => x.ShipmentId).NotEmpty();

        RuleFor(x => x.Status)
            .Must(s => s is null || Enum.IsDefined(typeof(MyFactory.Application.DTOs.Shipments.ShipmentStatus), s))
            .WithMessage("Invalid shipment status");

        RuleFor(x => x.ShipmentDate)
            .Must(d => d is null || d != default)
            .WithMessage("ShipmentDate must be a valid date when provided");

        When(x => x.Items is not null, () =>
        {
            RuleFor(x => x.Items!)
                .NotEmpty().WithMessage("If Items provided they cannot be empty");

            RuleForEach(x => x.Items!)
                .SetValidator(new CreateShipmentItemDtoValidator());
        });
    }
}
