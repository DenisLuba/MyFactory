using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.Sales;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Features.FinishedGoods.Common;

internal static class ReturnDtoFactory
{
    public static Task<ReturnDto> CreateAsync(
        IApplicationDbContext context,
        CustomerReturn customerReturn,
        CancellationToken cancellationToken)
    {
        return CreateAsync(context, new[] { customerReturn }, cancellationToken)
            .ContinueWith(task => task.Result.First(), cancellationToken);
    }

    public static async Task<IReadOnlyCollection<ReturnDto>> CreateAsync(
        IApplicationDbContext context,
        IReadOnlyCollection<CustomerReturn> returns,
        CancellationToken cancellationToken)
    {
        if (returns.Count == 0)
        {
            return Array.Empty<ReturnDto>();
        }

        var returnList = returns.ToList();

        var customerIds = returnList
            .Select(r => r.CustomerId)
            .Distinct()
            .ToList();

        var specificationIds = returnList
            .SelectMany(r => r.Items.Select(item => item.SpecificationId))
            .Distinct()
            .ToList();

        var customers = await context.Customers
            .AsNoTracking()
            .Where(customer => customerIds.Contains(customer.Id))
            .ToDictionaryAsync(customer => customer.Id, cancellationToken);

        var specifications = await context.Specifications
            .AsNoTracking()
            .Where(spec => specificationIds.Contains(spec.Id))
            .ToDictionaryAsync(spec => spec.Id, cancellationToken);

        return returnList
            .Select(r => Map(r, customers, specifications))
            .ToList();
    }

    private static ReturnDto Map(
        CustomerReturn customerReturn,
        IReadOnlyDictionary<Guid, Customer> customers,
        IReadOnlyDictionary<Guid, Specification> specifications)
    {
        var customerDto = customers.TryGetValue(customerReturn.CustomerId, out var customer)
            ? CustomerDto.FromEntity(customer)
            : new CustomerDto(customerReturn.CustomerId, string.Empty, string.Empty);

        var itemDtos = customerReturn.Items
            .Select(item => ReturnItemDto.FromEntity(
                item,
                specifications.TryGetValue(item.SpecificationId, out var specification) ? specification.Name : string.Empty))
            .ToList();

        return ReturnDto.FromEntity(customerReturn, customerDto, itemDtos);
    }
}
