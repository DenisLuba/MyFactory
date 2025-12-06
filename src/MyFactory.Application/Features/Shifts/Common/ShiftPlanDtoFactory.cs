using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shifts;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Application.Features.Shifts.Common;

internal static class ShiftPlanDtoFactory
{
    public static async Task<ShiftPlanDto> CreateAsync(
        IApplicationDbContext context,
        ShiftPlan plan,
        CancellationToken cancellationToken)
    {
        var plans = await CreateAsync(context, new[] { plan }, cancellationToken);
        return plans.First();
    }

    public static async Task<IReadOnlyCollection<ShiftPlanDto>> CreateAsync(
        IApplicationDbContext context,
        IReadOnlyCollection<ShiftPlan> plans,
        CancellationToken cancellationToken)
    {
        if (plans.Count == 0)
        {
            return Array.Empty<ShiftPlanDto>();
        }

        var planIds = plans
            .Select(plan => plan.Id)
            .Distinct()
            .ToList();

        var shiftResults = await context.ShiftResults
            .Where(result => planIds.Contains(result.ShiftPlanId))
            .OrderBy(result => result.RecordedAt)
            .ToListAsync(cancellationToken);

        var groupedResults = shiftResults
            .GroupBy(result => result.ShiftPlanId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyCollection<ShiftResult>)group.ToList());

        return plans
            .Select(plan =>
            {
                groupedResults.TryGetValue(plan.Id, out var results);
                return ShiftPlanDto.FromEntity(plan, results ?? Array.Empty<ShiftResult>());
            })
            .ToList();
    }
}
