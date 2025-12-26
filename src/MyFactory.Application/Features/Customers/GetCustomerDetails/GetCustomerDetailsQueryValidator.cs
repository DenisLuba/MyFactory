using FluentValidation;

namespace MyFactory.Application.Features.Customers.GetCustomerDetails;

public sealed class GetCustomerDetailsQueryValidator : AbstractValidator<GetCustomerDetailsQuery>
{
    public GetCustomerDetailsQueryValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}
