using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;
using MyFactory.Domain.Entities.Sales;

namespace MyFactory.Application.Features.FinishedGoods.Commands.CreateCustomerReturn;

public sealed class CreateCustomerReturnCommandHandler : IRequestHandler<CreateCustomerReturnCommand, ReturnDto>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerReturnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ReturnDto> Handle(CreateCustomerReturnCommand request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Return must contain at least one item.");
        }

        var customerExists = await _context.Customers
            .AsNoTracking()
            .AnyAsync(customer => customer.Id == request.CustomerId, cancellationToken);

        if (!customerExists)
        {
            throw new InvalidOperationException("Customer not found.");
        }

        var numberExists = await _context.CustomerReturns
            .AsNoTracking()
            .AnyAsync(entity => entity.ReturnNumber == request.ReturnNumber, cancellationToken);

        if (numberExists)
        {
            throw new InvalidOperationException("Return number already exists.");
        }

        var specificationIds = request.Items
            .Select(item => item.SpecificationId)
            .Distinct()
            .ToList();

        var specificationCount = await _context.Specifications
            .AsNoTracking()
            .CountAsync(specification => specificationIds.Contains(specification.Id), cancellationToken);

        if (specificationCount != specificationIds.Count)
        {
            throw new InvalidOperationException("One or more specifications were not found.");
        }

        var customerReturn = new CustomerReturn(request.ReturnNumber, request.CustomerId, request.ReturnDate, request.Reason);

        foreach (var item in request.Items)
        {
            customerReturn.AddItem(item.SpecificationId, item.Quantity, item.Disposition);
        }

        await _context.CustomerReturns.AddAsync(customerReturn, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return await ReturnDtoFactory.CreateAsync(_context, customerReturn, cancellationToken);
    }
}
