using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Shifts;
using MyFactory.Application.Features.Shifts.Common;
using MyFactory.Domain.Entities.Shifts;

namespace MyFactory.Application.OldFeatures.Shifts.Commands.CreateShiftPlan;

public sealed class CreateShiftPlanCommandHandler : IRequestHandler<CreateShiftPlanCommand, ShiftPlanDto>
{
    private readonly IApplicationDbContext _context;

    public CreateShiftPlanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftPlanDto> Handle(CreateShiftPlanCommand request, CancellationToken cancellationToken)
    {
        if (request.PlannedQuantity <= 0)
        {
            throw new InvalidOperationException("Planned quantity must be greater than zero.");
        }

        var employeeExists = await _context.Employees
            .AnyAsync(employee => employee.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var specificationExists = await _context.Specifications
            .AnyAsync(specification => specification.Id == request.SpecificationId, cancellationToken);

        if (!specificationExists)
        {
            throw new InvalidOperationException("Specification not found.");
        }

        var planExists = await _context.ShiftPlans
            .AnyAsync(plan => plan.EmployeeId == request.EmployeeId && plan.ShiftDate == request.ShiftDate, cancellationToken);

        if (planExists)
        {
            throw new InvalidOperationException("Shift plan already exists for this employee and date.");
        }

        var shiftPlan = new ShiftPlan(
            request.EmployeeId,
            request.SpecificationId,
            request.ShiftDate,
            request.ShiftType,
            request.PlannedQuantity);

        _context.ShiftPlans.Add(shiftPlan);
        await _context.SaveChangesAsync(cancellationToken);

        var planWithDetails = await _context.ShiftPlans
            .WithDetails()
            .AsNoTracking()
            .FirstAsync(plan => plan.Id == shiftPlan.Id, cancellationToken);

        return await ShiftPlanDtoFactory.CreateAsync(_context, planWithDetails, cancellationToken);
    }
}
