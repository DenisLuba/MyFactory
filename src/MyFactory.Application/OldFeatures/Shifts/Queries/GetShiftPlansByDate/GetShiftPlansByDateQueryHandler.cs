using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shifts;
using MyFactory.Application.Features.Shifts.Common;

namespace MyFactory.Application.OldFeatures.Shifts.Queries.GetShiftPlansByDate;

public sealed class GetShiftPlansByDateQueryHandler : IRequestHandler<GetShiftPlansByDateQuery, IReadOnlyCollection<ShiftPlanDto>>
{
    private readonly IApplicationDbContext _context;

    public GetShiftPlansByDateQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ShiftPlanDto>> Handle(GetShiftPlansByDateQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ShiftPlans
            .WithDetails()
            .AsNoTracking()
            .Where(plan => plan.ShiftDate == request.ShiftDate);

        if (request.EmployeeId.HasValue)
        {
            query = query.Where(plan => plan.EmployeeId == request.EmployeeId.Value);
        }

        var plans = await query
            .OrderBy(plan => plan.Employee!.FullName)
            .ThenBy(plan => plan.Specification!.Sku)
            .ToListAsync(cancellationToken);

        return await ShiftPlanDtoFactory.CreateAsync(_context, plans, cancellationToken);
    }
}
