using FluentValidation;

namespace MyFactory.Application.Features.Specifications.Commands.AddOperation;

public sealed class AddSpecificationOperationCommandValidator : AbstractValidator<AddSpecificationOperationCommand>
{
    public AddSpecificationOperationCommandValidator()
    {
        RuleFor(command => command.SpecificationId)
            .NotEmpty();

        RuleFor(command => command.OperationId)
            .NotEmpty();

        RuleFor(command => command.WorkshopId)
            .NotEmpty();

        RuleFor(command => command.TimeMinutes)
            .GreaterThan(0);

        RuleFor(command => command.OperationCost)
            .GreaterThan(0);
    }
}
