using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shifts;
using MyFactory.Application.Features.Shifts.Common;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Application.Features.Shifts.Commands.RecordShiftResult;

public sealed class RecordShiftResultCommandHandler : IRequestHandler<RecordShiftResultCommand, ShiftPlanDto>
{
    private readonly IApplicationDbContext _context;

    public RecordShiftResultCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftPlanDto> Handle(RecordShiftResultCommand request, CancellationToken cancellationToken)
    {
        if (request.HoursWorked <= 0)
        {
            throw new InvalidOperationException("Hours worked must be greater than zero.");
        }

        if (request.RecordedAt > DateTime.UtcNow)
        {
            throw new InvalidOperationException("Recorded date cannot be in the future.");
        }

        var plan = await _context.ShiftPlans
            .WithDetails()
            .FirstOrDefaultAsync(entity => entity.Id == request.ShiftPlanId, cancellationToken)
            ?? throw new InvalidOperationException("Shift plan not found.");

        var result = new ShiftResult(plan.Id, request.ActualQuantity, request.HoursWorked, request.RecordedAt);

        _context.ShiftResults.Add(result);
        await _context.SaveChangesAsync(cancellationToken);

        return await ShiftPlanDtoFactory.CreateAsync(_context, plan, cancellationToken);
    }
}
