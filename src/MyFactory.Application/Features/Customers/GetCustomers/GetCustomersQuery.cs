using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.Customers;

namespace MyFactory.Application.Features.Customers.GetCustomers;

public sealed record GetCustomersQuery(
    string? SearchName = null,
    string? SortBy = null,
    bool SortDesk = false,
    int Skip = 0,
    int Take = 30,
    bool? IsActive = null
) : IRequest<ListDto<CustomerListItemDto>>;
