using MediatR;

namespace MyFactory.Application.Features.Customers.DeactivateCustomer;

public sealed record DeactivateCustomerCommand(Guid CustomerId) : IRequest;
