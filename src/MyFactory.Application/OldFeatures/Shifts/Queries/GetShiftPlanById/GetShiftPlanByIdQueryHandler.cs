using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shifts;
using MyFactory.Application.Features.Shifts.Common;

namespace MyFactory.Application.OldFeatures.Shifts.Queries.GetShiftPlanById;

public sealed class GetShiftPlanByIdQueryHandler : IRequestHandler<GetShiftPlanByIdQuery, ShiftPlanDto>
{
    private readonly IApplicationDbContext _context;

    public GetShiftPlanByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftPlanDto> Handle(GetShiftPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _context.ShiftPlans
            .WithDetails()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.ShiftPlanId, cancellationToken)
            ?? throw new InvalidOperationException("Shift plan not found.");

        return await ShiftPlanDtoFactory.CreateAsync(_context, plan, cancellationToken);
    }
}
