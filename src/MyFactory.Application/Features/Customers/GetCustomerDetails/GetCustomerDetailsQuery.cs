using MediatR;
using MyFactory.Application.DTOs.Customers;

namespace MyFactory.Application.Features.Customers.GetCustomerDetails;

public sealed record GetCustomerDetailsQuery(Guid CustomerId)
    : IRequest<CustomerDetailsDto>;
