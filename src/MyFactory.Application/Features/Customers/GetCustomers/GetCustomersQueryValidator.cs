using FluentValidation;

namespace MyFactory.Application.Features.Customers.GetCustomers;

public sealed class GetCustomersQueryValidator : AbstractValidator<GetCustomersQuery>
{
    public GetCustomersQueryValidator()
    {
    }
}
