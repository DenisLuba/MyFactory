using FluentValidation;

namespace MyFactory.Application.Features.Warehouses.TransferMaterials;

public sealed class TransferMaterialsCommandValidator
    : AbstractValidator<TransferMaterialsCommand>
{
    public TransferMaterialsCommandValidator()
    {
        RuleFor(x => x.FromWarehouseId).NotEmpty();
        RuleFor(x => x.ToWarehouseId).NotEmpty();

        RuleFor(x => x)
            .Must(x => x.FromWarehouseId != x.ToWarehouseId)
            .WithMessage("Source and destination warehouses must be different.");

        RuleFor(x => x.Items)
            .NotEmpty();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.MaterialId).NotEmpty();
            item.RuleFor(i => i.Qty).GreaterThan(0);
        });
    }
}