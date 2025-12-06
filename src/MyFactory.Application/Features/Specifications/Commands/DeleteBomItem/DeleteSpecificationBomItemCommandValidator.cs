using FluentValidation;

namespace MyFactory.Application.Features.Specifications.Commands.DeleteBomItem;

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
