using FluentValidation;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.DeleteBomItem;

public sealed class DeleteSpecificationBomItemCommandValidator : AbstractValidator<DeleteSpecificationBomItemCommand>
{
    public DeleteSpecificationBomItemCommandValidator()
    {
        RuleFor(command => command.SpecificationId)
            .NotEmpty();

        RuleFor(command => command.BomItemId)
            .NotEmpty();
    }
}
