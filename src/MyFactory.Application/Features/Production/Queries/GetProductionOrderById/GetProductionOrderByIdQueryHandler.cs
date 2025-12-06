using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Application.Features.Production.Common;

namespace MyFactory.Application.Features.Production.Queries.GetProductionOrderById;

public sealed class GetProductionOrderByIdQueryHandler : IRequestHandler<GetProductionOrderByIdQuery, ProductionOrderDto>
{
    private readonly IApplicationDbContext _context;

    public GetProductionOrderByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductionOrderDto> Handle(GetProductionOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.ProductionOrders
            .WithDetails()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Production order not found.");

        if (order.Stages.Any() && order.Stages.First().Assignments.Count == 0)
        {
            throw new InvalidOperationException("Assignments not loaded with production order.");
        }

        return await ProductionOrderDtoFactory.CreateAsync(_context, order, cancellationToken);
    }
}
