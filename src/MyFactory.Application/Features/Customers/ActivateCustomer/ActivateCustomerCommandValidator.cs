using FluentValidation;

namespace MyFactory.Application.Features.Customers.ActivateCustomer;

public sealed class ActivateCustomerCommandValidator : AbstractValidator<ActivateCustomerCommand>
{
    public ActivateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
