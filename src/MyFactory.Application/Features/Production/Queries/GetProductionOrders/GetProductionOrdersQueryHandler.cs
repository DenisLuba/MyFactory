using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Production;
using MyFactory.Application.Features.Production.Common;

namespace MyFactory.Application.Features.Production.Queries.GetProductionOrders;

public sealed class GetProductionOrdersQueryHandler : IRequestHandler<GetProductionOrdersQuery, IReadOnlyCollection<ProductionOrderDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProductionOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ProductionOrderDto>> Handle(GetProductionOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ProductionOrders
            .WithDetails()
            .AsNoTracking()
            .AsQueryable();

        if (request.Status.HasValue)
        {
            query = query.Where(order => order.Status == request.Status.Value);
        }

        if (request.SpecificationId.HasValue)
        {
            query = query.Where(order => order.SpecificationId == request.SpecificationId.Value);
        }

        var orders = await query
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);

        return await ProductionOrderDtoFactory.CreateAsync(_context, orders, cancellationToken);
    }
}
