using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Application.Features.Production.Common;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.OldFeatures.Production.Commands.CreateProductionOrder;

public sealed class CreateProductionOrderCommandHandler : IRequestHandler<CreateProductionOrderCommand, ProductionOrderDto>
{
    private readonly IApplicationDbContext _context;

    public CreateProductionOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductionOrderDto> Handle(CreateProductionOrderCommand request, CancellationToken cancellationToken)
    {
        var specificationExists = await _context.Specifications
            .AnyAsync(specification => specification.Id == request.SpecificationId, cancellationToken);

        if (!specificationExists)
        {
            throw new InvalidOperationException("Specification not found.");
        }

        var numberAlreadyUsed = await _context.ProductionOrders
            .AnyAsync(order => order.OrderNumber == request.OrderNumber, cancellationToken);

        if (numberAlreadyUsed)
        {
            throw new InvalidOperationException("Production order number already exists.");
        }

        var order = ProductionOrder.Create(request.OrderNumber, request.SpecificationId, request.QtyOrdered, DateTime.UtcNow);

        _context.ProductionOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return await ProductionOrderDtoFactory.CreateAsync(_context, order, cancellationToken);
    }
}
