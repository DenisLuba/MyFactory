using FluentValidation;
using MyFactory.Application.Features.FinishedGoods.Commands.CreateCustomerReturn;

namespace MyFactory.Application.Features.FinishedGoods.Validators;

public sealed class CreateCustomerReturnCommandValidator : AbstractValidator<CreateCustomerReturnCommand>
{
    public CreateCustomerReturnCommandValidator()
    {
        RuleFor(command => command.ReturnNumber).NotEmpty();
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.Reason).NotEmpty();
        RuleFor(command => command.ReturnDate)
            .Must(date => date != default)
            .WithMessage("Return date is required.");
        RuleFor(command => command.Items)
            .NotEmpty()
            .WithMessage("Return must contain at least one item.");

        RuleForEach(command => command.Items)
            .SetValidator(new CreateCustomerReturnItemDtoValidator());
    }

    private sealed class CreateCustomerReturnItemDtoValidator : AbstractValidator<CreateCustomerReturnItemDto>
    {
        public CreateCustomerReturnItemDtoValidator()
        {
            RuleFor(item => item.SpecificationId).NotEmpty();
            RuleFor(item => item.Quantity).GreaterThan(0m);
            RuleFor(item => item.Disposition).NotEmpty();
        }
    }
}
