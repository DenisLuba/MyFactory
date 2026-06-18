using MediatR;

namespace MyFactory.Application.Features.Customers.ActivateCustomer;

public sealed record ActivateCustomerCommand(Guid CustomerId) : IRequest;
