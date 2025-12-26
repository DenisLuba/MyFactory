using MediatR;

namespace MyFactory.Application.Features.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand(
    Guid CustomerId,
    string Name,
    string? Phone,
    string? Email,
    string? Address
) : IRequest;
