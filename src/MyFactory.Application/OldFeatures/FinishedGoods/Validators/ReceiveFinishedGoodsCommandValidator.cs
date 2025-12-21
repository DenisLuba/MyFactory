using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Commands.ReceiveFinishedGoods;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Validators;

public sealed class ReceiveFinishedGoodsCommandValidator : AbstractValidator<ReceiveFinishedGoodsCommand>
{
    public ReceiveFinishedGoodsCommandValidator()
    {
        RuleFor(command => command.SpecificationId).NotEmpty();
        RuleFor(command => command.WarehouseId).NotEmpty();
        RuleFor(command => command.Quantity).GreaterThan(0m);
        RuleFor(command => command.UnitCost).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.ReceivedAt)
            .Must(date => date != default)
            .WithMessage("Received date is required.");
    }
}
