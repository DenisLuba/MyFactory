using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Common;

internal static class ShiftPlanQueryableExtensions
{
    public static IQueryable<ShiftPlan> WithDetails(this IQueryable<ShiftPlan> query)
    {
        return query
            .Include(plan => plan.Employee)
            .Include(plan => plan.Specification);
    }
}
