using MediatR;
using MyFactory.Application.DTOs.Customers;

namespace MyFactory.Application.Features.Customers.GetCustomers;

public sealed record GetCustomersQuery : IRequest<IReadOnlyList<CustomerListItemDto>>;
