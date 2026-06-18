using FluentValidation;

namespace MyFactory.Application.Features.Customers.GetCustomerCard;

public sealed class GetCustomerCardQueryValidator : AbstractValidator<GetCustomerCardQuery>
{
    public GetCustomerCardQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
