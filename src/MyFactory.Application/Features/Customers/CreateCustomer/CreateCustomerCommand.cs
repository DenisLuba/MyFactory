using MediatR;

namespace MyFactory.Application.Features.Customers.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string? Phone,
    string? Email,
    string? Address
) : IRequest<Guid>;
