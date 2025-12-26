using MediatR;
using MyFactory.Application.DTOs.Customers;

namespace MyFactory.Application.Features.Customers.GetCustomerCard;

public sealed record GetCustomerCardQuery(Guid CustomerId)
    : IRequest<CustomerCardDto>;
