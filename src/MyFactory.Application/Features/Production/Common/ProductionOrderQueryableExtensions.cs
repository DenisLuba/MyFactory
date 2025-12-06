using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.Production.Common;

internal static class ProductionOrderQueryableExtensions
{
    public static IQueryable<ProductionOrder> WithDetails(this IQueryable<ProductionOrder> query)
    {
        return query
            .Include(order => order.Allocations)
            .Include(order => order.Stages)
                .ThenInclude(stage => stage.Assignments);
    }
}
