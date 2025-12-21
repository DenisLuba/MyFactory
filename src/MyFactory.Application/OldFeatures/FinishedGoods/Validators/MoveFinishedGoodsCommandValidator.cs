using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Commands.MoveFinishedGoods;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Validators;

public sealed class MoveFinishedGoodsCommandValidator : AbstractValidator<MoveFinishedGoodsCommand>
{
    public MoveFinishedGoodsCommandValidator()
    {
        RuleFor(command => command.SpecificationId).NotEmpty();
        RuleFor(command => command.FromWarehouseId).NotEmpty();
        RuleFor(command => command.ToWarehouseId).NotEmpty();
        RuleFor(command => command.Quantity).GreaterThan(0m);
        RuleFor(command => command.MovedAt)
            .Must(date => date != default)
            .WithMessage("Movement date is required.");
        RuleFor(command => command)
            .Must(command => command.FromWarehouseId != command.ToWarehouseId)
            .WithMessage("Source and destination warehouses must differ.");
    }
}
